""" Upload to BellRichM weather client. """
import json

try:
    import queue as Queue
except ImportError:
    import Queue

import ssl
import time

import jwt
import six

import weewx
import weewx.restx

from six.moves import urllib

# hack to use ssl when running locally
ssl._create_default_https_context = ssl._create_unverified_context #pylint: disable=protected-access

VERSION = "0.23"

if weewx.__version__ < "3":
    raise weewx.UnsupportedFeature("weewx 3 is required, found %s" %
                                   weewx.__version__)

try:
    # Test for new-style weewx logging by trying to import weeutil.logger
    import weeutil.logger # pylint: disable=unused-import
    import logging
    LOG = logging.getLogger(__name__)

    def logdbg(msg):
        """ Log debug """
        print(msg)
        LOG.debug(msg)

    def loginf(msg):
        """ Log info """
        print(msg)
        LOG.info(msg)

    def logerr(msg):
        """ Log error """
        print(msg)
        LOG.error(msg)

except ImportError:
    # Old-style weewx logging
    import syslog

    def logmsg(level, msg):
        """ Log the message """
        print(msg)
        syslog.syslog(level, 'weert: %s:' % msg)

    def logdbg(msg):
        """ Log debug """
        logmsg(syslog.LOG_DEBUG, msg)

    def loginf(msg):
        """ Log info """
        logmsg(syslog.LOG_INFO, msg)

    def logerr(msg):
        """ Log error """
        logmsg(syslog.LOG_ERR, msg)

class Uploader(weewx.restx.StdRESTbase):
    """ The BellRichM uploader. """
    def __init__(self, engine, config_dict):
        super(Uploader, self).__init__(engine, config_dict)
        loginf("service version is %s" % VERSION)

        site_dict = weewx.restx.check_enable(config_dict, 'Uploader', 'host', 'user', 'password')
        if site_dict is None:
            return

        host = site_dict['host']
        user = site_dict['user']
        password = site_dict['password']

        manager_dict = weewx.manager.get_manager_dict_from_config(
            config_dict, 'wx_binding')

        self.archive_queue = Queue.Queue()
        self.archive_thread = UploaderThread(self.archive_queue,
                                             host, user, password,
                                             manager_dict)
        self.archive_thread.start()

        self.bind(weewx.NEW_ARCHIVE_RECORD, self.new_archive_record)

    def new_archive_record(self, event):
        " New archive record callback "
        self.archive_queue.put(event.record)

class UploaderThread(weewx.restx.RESTThread):
    """ The uploader thread. """
    def __init__(self, queue, host, user, password,
                 manager_dict=None,
                 post_interval=None, max_backlog=six.MAXSIZE, stale=None,
                 log_success=True, log_failure=True,
                 timeout=10, max_tries=3, retry_wait=5, retry_login=3600, retry_certificate=3600,
                 softwaretype="weewx-%s" % weewx.__version__,
                 skip_upload=False):

        super(UploaderThread, self).__init__(queue,
                                             protocol_name='Uploader',
                                             manager_dict=manager_dict,
                                             max_backlog=max_backlog,
                                             stale=stale,
                                             log_success=log_success,
                                             log_failure=log_failure,
                                             max_tries=max_tries,
                                             timeout=timeout,
                                             retry_wait=retry_wait)

        self.host = host
        self.login = UploaderLogin(Queue.Queue(), self.host, user, password)
        self.jwt = self.login.process_record(None, None)
        self.interval = 300

    def process_record(self, record, dbmanager):
        if self.jwt['decoded']['exp'] < int(time.time()) + self.interval:
            self.jwt = self.login.process_record(None, None)

        super().process_record(record, dbmanager)

    def format_url(self, _):
        url = "https://%s/api/observations" % self.host
        return url

    def get_request(self, url):
        _request = super().get_request(url)
        _request.add_header("authorization", "bearer " + self.jwt['encoded'])
        return _request

    def get_post_body(self, record):
        record['dateTime'] = int(record['dateTime'])
        record['usUnits'] = int(record['usUnits'])
        record['interval'] = int(record['interval'])
        return(json.dumps(record), "application/json")

    def handle_exception(self, e, count):
        logdbg("%s: Failed upload attempt %d: %s" % (self.protocol_name, count, e))
        if isinstance(e, urllib.error.HTTPError):
            response = e.file
            response_body = response.read()
            logerr("%s: Failed upload attempt %d: %s" % (self.protocol_name, count, response_body))

class UploaderLogin(weewx.restx.RESTThread):
    """ The login thread """
    def __init__(self, queue,
                 host,
                 user, password,
                 #measurement,
                 #platform, stream,
                 ## loop_filters,
                 protocol_name="RMBArchiveUploadLogin",
                 post_interval=None, max_backlog=six.MAXSIZE, stale=None,
                 log_success=True, log_failure=True,
                 timeout=10, max_tries=3, retry_wait=5, retry_login=3600,
                 softwaretype="weewx-%s" % weewx.__version__,
                 skip_upload=False):

        super(UploaderLogin, self).__init__(queue,
                                            protocol_name=protocol_name,
                                            post_interval=post_interval,
                                            max_backlog=max_backlog,
                                            stale=stale,
                                            log_success=log_success,
                                            log_failure=log_failure,
                                            timeout=timeout,
                                            max_tries=max_tries,
                                            retry_wait=retry_wait,
                                            retry_login=retry_login,
                                            softwaretype=softwaretype,
                                            skip_upload=skip_upload)

        self.host = host
        #self.port = to_int(port)
        self.user = user
        self.password = password

        self.jwt = {}
        self.jwt['encoded'] = None
        self.jwt['decoded'] = {}
        self.jwt['decoded']['exp'] = 0

    def process_record(self, record, dbmanager):
        # ... format the URL, using the relevant protocol ...
        _url = self.format_url(record)
        # ... get the Request to go with it...
        _request = self.get_request(_url)
        #  ... get any POST payload...
        _payload = self.get_post_body(record)
        # ... add a proper Content-Type if needed...
        if _payload:
            _request.add_header('Content-Type', _payload[1])
            data = _payload[0]
        else:
            data = None

        # ... then, finally, post it
        self.post_with_retries(_request, data)

        return self.jwt

    def format_url(self, _):
        #url = "%s %s %s" % (self.host, self.port, self.measurement)
        url = "https://%s/api/user/login" % self.host
        return url

    def get_post_body(self, record):
        data = {}
        data['UserName'] = self.user
        data['Password'] = self.password
        return(json.dumps(data), "application/json")

    def check_response(self, response):
        # Get the token
        response_body = response.read()
        data = json.loads(response_body)
        self.jwt['encoded'] = data['jsonWebToken']
        self.jwt['decoded'] = jwt.decode(data['jsonWebToken'], verify=False)
        print("check response %s" % self.jwt)

    def handle_exception(self, e, count):
        logdbg("%s: Failed upload attempt %d: %s" % (self.protocol_name, count, e))
        if isinstance(e, urllib.error.HTTPError):
            response = e.file
            response_body = response.read()
            logerr("%s: Failed upload attempt %d: %s" % (self.protocol_name, count, response_body))

class Observation(weewx.restx.RESTThread):
    """ Manage BellRichM onservations. """
    def __init__(self, queue, host, user, password,
                 manager_dict=None,
                 post_interval=None, max_backlog=six.MAXSIZE, stale=None,
                 log_success=True, log_failure=True,
                 timeout=10, max_tries=3, retry_wait=5, retry_login=3600, retry_certificate=3600,
                 softwaretype="weewx-%s" % weewx.__version__,
                 skip_upload=False):

        super(Observation, self).__init__(queue,
                                          protocol_name='UploaderObservations',
                                          manager_dict=manager_dict,
                                          max_backlog=max_backlog,
                                          stale=stale,
                                          log_success=log_success,
                                          log_failure=log_failure,
                                          max_tries=max_tries,
                                          timeout=timeout,
                                          retry_wait=retry_wait)
        self.host = host
        self.login = UploaderLogin(Queue.Queue(), self.host, user, password)
        self.jwt = self.login.process_record(None, None)
        self.interval = 300

    def process_record(self, record, dbmanager):
        if self.jwt['decoded']['exp'] < int(time.time()) + self.interval:
            self.jwt = self.login.process_record(None, None)

        super().process_record(record, dbmanager)

    def format_url(self, _):
        url = "https://%s/api/Observations/Timestamps" % self.host
        return url

    def get_post_body(self, record):
        data = {}
        data['StartDateTime'] = int(record['StartDateTime'])
        data['EndDateTime'] = int(record['EndDateTime'])
        return(json.dumps(data), "application/json")

    def get_request(self, url):
        _request = super().get_request(url)
        _request.add_header("authorization", "bearer " + self.jwt['encoded'])
        _request.method = 'GET'
        return _request

    def check_response(self, response):
        response_body = response.read()
        self.data = json.loads(response_body)


    def handle_exception(self, e, count):
        logdbg("%s: Failed upload attempt %d: %s" % (self.protocol_name, count, e))
        if isinstance(e, urllib.error.HTTPError):
            response = e.file
            response_body = response.read()
            logerr("%s: Failed upload attempt %d: %s" % (self.protocol_name, count, response_body))

if __name__ == '__main__':
    import configobj
    import datetime
    import os

    def main():
        """ mainline """
        print("in main")

        config_file = 'weewx.conf'
        config_path = os.path.abspath(config_file)

        config_dict = configobj.ConfigObj(config_path, file_error=True)

        site_dict = weewx.restx.check_enable(config_dict, 'Uploader', 'host', 'user', 'password')
        if site_dict is None:
            return

        host = site_dict['host']
        user = site_dict['user']
        password = site_dict['password']

        db_binding = 'wx_binding'

        dbmanager = weewx.manager.open_manager_with_config(config_dict, db_binding)

        date = '2020-09-26'
        # 1601092800 Saturday, September 26, 2020 12:00:00 AM GMT-04:00 DST
        # 1601178900  Saturday, September 26, 2020 11:55:00 PM GMT-04:00 DST

        date = '2020-09-27'
        # 1601179200 Sunday, September 27, 2020 12:00:00 AM GMT-04:00 DST
        # 1601265300 Sunday, September 27, 2020 11:55:00 PM GMT-04:00 DST

        date_tt = time.strptime(date, "%Y-%m-%d")
        date_date = datetime.date(date_tt[0], date_tt[1], date_tt[2])

        start_ord = date_date.toordinal()
        end_ord = start_ord + 1

        start_date = datetime.date.fromordinal(start_ord)
        end_date = datetime.date.fromordinal(end_ord)

        start_ts = time.mktime(start_date.timetuple())
        end_ts = time.mktime(end_date.timetuple())

        sql_stmt = "SELECT dateTime FROM archive WHERE dateTime>=? AND dateTime<?"

        records = dbmanager.genSql(sql_stmt, (start_ts, end_ts))
        weewx_timestamps = []
        for record in records:
            weewx_timestamps.append(record[0])

        observation = Observation(None,
                                  host, user, password)
                                  # None, {})

        data = {}
        data['StartDateTime'] = start_ts
        data['EndDateTime'] = end_ts
        observation.process_record(data, None)
        client_timestamps = []
        for timestamp in observation.data:
            client_timestamps.append(timestamp['dateTime'])

        missing_timestamps = sorted(set(weewx_timestamps).difference(client_timestamps))

        uploader = UploaderThread(None,
                                  host, user, password,
                                  dbmanager)

        for timestamp in missing_timestamps:
            record = dbmanager.getRecord(timestamp)
            uploader.process_record(record, dbmanager)

        print("done")

    main()
