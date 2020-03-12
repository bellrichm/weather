""" The uploader """
# pylint: disable=invalid-name
# pylint: enable=invalid-name
import json
import sys
import time
import weewx.restx
#from weeutil.weeutil import to_int


try:
    # Python 2
    from Queue import Queue
except ImportError:
    # Python 3
    from queue import Queue

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

class RMBArchiveUpload(weewx.restx.StdRESTful):
    """ The uploader class """
    def __init__(self, engine, config_dict):
        super(RMBArchiveUpload, self).__init__(engine, config_dict)
        loginf("init RMBArchiveUpload")

        archive_upload_manager_dict = weewx.manager.get_manager_dict(
            config_dict['DataBindings'],
            config_dict['Databases'],
            'RMBArchiveUpload_binding')
        self.archive_upload_DBM = weewx.manager.open_manager(archive_upload_manager_dict, # pylint: disable=invalid-name
                                                             initialize=True)

        site_dict = weewx.restx.check_enable(config_dict, 'RmbUpload', 'host', 'user', 'password')
        if site_dict is None:
            return

        manager_dict = weewx.manager.get_manager_dict_from_config(
            config_dict, 'wx_binding')

        self.archive_queue = Queue()
        self.archive_thread = RMBArchiveUploadThread(self.archive_queue,
                                                     #manager_dict,
                                                     archive_upload_manager_dict,
                                                     **site_dict)
        self.archive_thread.start()

        self.bind(weewx.NEW_ARCHIVE_RECORD, self.new_archive_record)

    def new_archive_record(self, event):
        " New archive record callback "
        # Adding to DB here, incase the queuing fails
        self.archive_upload_DBM.getSql('INSERT INTO %s ("dateTime", "run_dateTime") VALUES (?, ?)' %
                                       self.archive_upload_DBM.table_name,
                                       (str(event.record["dateTime"]),
                                        str(int(time.time()))))


        self.archive_queue.put(event.record)

        loginf("new archive record %s" % event)

class RMBArchiveUploadThread(weewx.restx.RESTThread):
    """ The uploader thread """
    def __init__(self, queue,
                 #manager_dict,
                 archiveUpload_manager_dict,
                 host,
                 user, password,
                 #measurement,
                 #platform, stream,
                 ## loop_filters,
                 protocol_name="RMBArchiveUploadThread",
                 post_interval=None, max_backlog=sys.maxsize, stale=None,
                 log_success=True, log_failure=True,
                 timeout=10, max_tries=3, retry_wait=5, retry_login=3600,
                 softwaretype="weewx-%s" % weewx.__version__,
                 skip_upload=False):
        """ Initializer for RMBArchiveUploadThread """

        super(RMBArchiveUploadThread, self).__init__(queue,
                                                     #manager_dict,
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
        #self.user = user
        #self.password = password

        self.archive_upload_manager_dict = archiveUpload_manager_dict
        self.archive_upload_db_manager = None

        self.login = RMBArchiveUploadLogin(Queue(), self.host, user, password)
        self.jwt = self.login.process_record(None, None)

        #self.measurement = measurement
        #self.platform = platform
        #self.stream = stream
        loginf("init RMBArchiveUploadThread")

    def process_record(self, record, dbmanager):
        # Constructor is a different thread, so have to do this here.
        if not self.archive_upload_db_manager:
            self.archive_upload_db_manager = weewx.manager.open_manager(
                self.archive_upload_manager_dict)

        super().process_record(record, dbmanager)

        curr_date_time = int(time.time())

        self.archive_upload_db_manager.getSql(
            'UPDATE %s SET upload_dateTime = ? WHERE dateTime= ?' %
            self.archive_upload_db_manager.table_name,
            (str(curr_date_time),
             record["dateTime"]))

        #setup for next call
        # ToDo what about if there is an error?
        #self.jwt = self.login.process_record(None, None)
        loginf("process_record")

    def format_url(self, _):
        """Override and return the URL used to post to the server"""

        url = "http://%s/api/observations" % self.host
        return url

    def get_request(self, url):
        _request = super().get_request(url)
        _request.add_header("authorization", "bearer " + self.jwt)
        return _request

    def get_post_body(self, record):
        record['dateTime'] = int(record['dateTime'])
        record['usUnits'] = int(record['usUnits'])
        record['interval'] = int(record['interval'])
        print(record['interval'])
        return(json.dumps(record), "application/json")

class RMBArchiveUploadLogin(weewx.restx.RESTThread):
    """ The login thread """
    def __init__(self, queue,
                 host,
                 user, password,
                 #measurement,
                 #platform, stream,
                 ## loop_filters,
                 protocol_name="RMBArchiveUploadLogin",
                 post_interval=None, max_backlog=sys.maxsize, stale=None,
                 log_success=True, log_failure=True,
                 timeout=10, max_tries=3, retry_wait=5, retry_login=3600,
                 softwaretype="weewx-%s" % weewx.__version__,
                 skip_upload=False):
        """ Initializer for RMBArchiveUploadLogin """

        super(RMBArchiveUploadLogin, self).__init__(queue,
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

        self.jwt = None
        print("init login")

    def process_record(self, record, dbmanager):
        loginf("process_record")

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
        """Override and return the URL used to post to the WeeRT server"""

        #url = "%s %s %s" % (self.host, self.port, self.measurement)
        url = "http://%s/api/user/login" % self.host
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
        self.jwt = data['jsonWebToken']
        print("check response %s" % self.jwt)


if __name__ == '__main__':
    import argparse
    import os

    import configobj

    from weewx.engine import StdEngine

    def main():
        """ mainline """
        print("in main")
        parser = argparse.ArgumentParser()
        parser.add_argument("config_file")

        options = parser.parse_args()

        config_path = os.path.abspath(options.config_file)

        config_dict = configobj.ConfigObj(config_path, file_error=True)

        min_config_dict = {
            'Station': {
                'altitude': [0, 'foot'],
                'latitude': 0,
                'station_type': 'Simulator',
                'longitude': 0
            },
            'Simulator': {
                'driver': 'weewx.drivers.simulator',
            },
            'Engine': {
                'Services': {}
            }
        }
        engine = StdEngine(min_config_dict)

        service = RMBArchiveUpload(engine, config_dict)

        db_binder = weewx.manager.DBBinder(config_dict)
        data_binding = config_dict['StdArchive'].get('data_binding', 'wx_binding')
        dbmanager = db_binder.get_manager(data_binding)

        record = {}
        record['dateTime'] = int(time.time())
        record['usUnits'] = 1
        service.archive_thread.process_record(record, dbmanager)

        print("done")

    main()
