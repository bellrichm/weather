""" The uploader """
# pylint: disable=invalid-name
# pylint: enable=invalid-name
import json
import sys
import time
import jwt
import weewx.restx
#from weeutil.weeutil import to_int

#import six
from six.moves import urllib

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
                                                     manager_dict,
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
                 manager_dict,
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
                                                     manager_dict=manager_dict,
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
        self.interval = 300

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

        if self.jwt['decoded']['exp'] < int(time.time()) + self.interval:
            self.jwt = self.login.process_record(None, None)

        super().process_record(record, dbmanager)

        curr_date_time = int(time.time())

        self.archive_upload_db_manager.getSql(
            'UPDATE %s SET upload_dateTime = ? WHERE dateTime= ?' %
            self.archive_upload_db_manager.table_name,
            (str(curr_date_time),
             record["dateTime"]))

        loginf("process_record")

    def format_url(self, _):
        """Override and return the URL used to post to the server"""

        url = "http://%s/api/observations" % self.host
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
        #super().handle_exception(e, count)
        print(type(e))
        if isinstance(e, urllib.error.HTTPError):
            response = e.file
            response_body = response.read()
            logerr("%s: Failed upload attempt %d: %s" % (self.protocol_name, count, response_body))
        print("exception")

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

        self.jwt = {}
        self.jwt['encoded'] = None
        self.jwt['decoded'] = {}
        self.jwt['decoded']['exp'] = 0
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
        self.jwt['encoded'] = data['jsonWebToken']
        self.jwt['decoded'] = jwt.decode(data['jsonWebToken'], verify=False)
        print("check response %s" % self.jwt)

    def handle_exception(self, e, count):
        logdbg("%s: Failed upload attempt %d: %s" % (self.protocol_name, count, e))
        #super().handle_exception(e, count)
        print(type(e))
        if isinstance(e, urllib.error.HTTPError):
            response = e.file
            response_body = response.read()
            logerr("%s: Failed upload attempt %d: %s" % (self.protocol_name, count, response_body))

if __name__ == '__main__':
    import argparse
    import copy
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

        #service = RMBArchiveUpload(engine, config_dict)

        db_binder = weewx.manager.DBBinder(config_dict)
        data_binding = config_dict['StdArchive'].get('data_binding', 'wx_binding')
        dbmanager = db_binder.get_manager(data_binding)

        record = {}
        record['dateTime'] = time.time()
        record['usUnits'] = 1
        record['interval'] = 5

        records = catch_up2(config_dict)
        for record in records:
            print(record)

        #service.archive_thread.process_record(record, dbmanager)

        print("done")

    def catch_up2(config_dict):
        dictionary = copy.deepcopy(config_dict)
        dictionary['StdRESTful']['RmbUpload']['max_tries'] = '3'
        dictionary['StdRESTful']['RmbUpload']['retry_wait'] = '5'

        site_dict = weewx.restx.check_enable(dictionary, 'RmbUpload', 'user', 'password')
        if site_dict is None:
            return

        archive_upload_manager_dict = weewx.manager.get_manager_dict(
            config_dict['DataBindings'], config_dict['Databases'], 'RMBArchiveUpload_binding')
        site_dict['archiveUpload_manager_dict'] = archive_upload_manager_dict

        archive_upload_manager = weewx.manager.open_manager(archive_upload_manager_dict)

        select_sql = "SELECT dateTime from archive where archive.upload_dateTime is NULL"

        error_dates = archive_upload_manager.genSql(select_sql)

        lister = []
        for error_date in error_dates:
            lister.append(error_date[0])

        placeholder = '?' # For SQLite. See DBAPI paramstyle.
        placeholders = ', '.join(placeholder for unused in lister)
        query = 'SELECT * FROM archive WHERE dateTime IN (%s)' % placeholders
        #cursor.execute(query, error_dates)

        db_binder = weewx.manager.DBBinder(config_dict)
        data_binding = config_dict['StdArchive'].get('data_binding', 'wx_binding')
        dbmanager = db_binder.get_manager(data_binding)

        records = dbmanager.genSql(query, lister)

        print("end")
        return records

    def catch_up(config_dict):
        """ process any ones that have errored
            still need to think about ones that were neverr logged
            probably need to retrieve observations from server and compare to db
            similar to wunderfixer """
        # Unfortunately, this is dependent on the underlying databases being SQLite
        dictionary = copy.deepcopy(config_dict)
        dictionary['StdRESTful']['RmbUpload']['max_tries'] = '3'
        dictionary['StdRESTful']['RmbUpload']['retry_wait'] = '5'

        attach_sql = "ATTACH DATABASE \
            '/home/richbell/development/weewx-code/weewx/archive-replica/weewx.sdb' \
            as weewx;"
        # gets all archive data that does not have a record stating int was processed
        # where_clause = "WHERE weewx.archive.dateTime IN \
        #     (SELECT dateTime FROM archive where archive.upload_dateTime is NULL) "
        # gets all archive data that has not been marked as processed
        # where_clause = "WHERE weewx.archive.dateTime NOT IN (SELECT dateTime FROM archive) "
        select_sql = "SELECT \
                        `dateTime`, `usUnits`, `interval`, `barometer`, `pressure`, `altimeter`, `inTemp`, `outTemp`, \
                        `inHumidity`, `outHumidity`, `windSpeed`, `windDir`, `windGust`, `windGustDir`, \
                        `rainRate`, `rain`, `dewpoint`, `windchill`, `heatindex`, `ET`, `radiation`, `UV`, \
                        `extraTemp1`, `extraTemp2`, `extraTemp3`, `soilTemp1`, `soilTemp2`, `soilTemp3`, `soilTemp4`, \
                        `leafTemp1`, `leafTemp2`, `extraHumid1`, `extraHumid2`, `soilMoist1`, `soilMoist2`, `soilMoist3`, `soilMoist4`, \
                        `leafWet1`, `leafWet2`, `rxCheckPercent`, `txBatteryStatus`, `consBatteryVoltage`, `hail`, `hailRate`, \
                        `heatingTemp`, `heatingVoltage`, `supplyVoltage`, `referenceVoltage`, \
                        `windBatteryStatus`, `rainBatteryStatus`, `outTempBatteryStatus`, `inTempBatteryStatus` \
            FROM weewx.archive \
                WHERE weewx.archive.dateTime IN (SELECT dateTime FROM archive where archive.upload_dateTime is NULL) \
                OR weewx.archive.dateTime IN (SELECT dateTime FROM archive where archive.upload_dateTime is NULL) \
                ORDER BY dateTime ASC ;"

        site_dict = weewx.restx.check_enable(dictionary, 'RmbUpload', 'user', 'password')
        if site_dict is None:
            return

        archive_upload_manager_dict = weewx.manager.get_manager_dict(
            config_dict['DataBindings'], config_dict['Databases'], 'RMBArchiveUpload_binding')
        site_dict['archiveUpload_manager_dict'] = archive_upload_manager_dict

        archive_upload_manager = weewx.manager.open_manager(archive_upload_manager_dict)

        archive_upload_manager.getSql(attach_sql)

        data_records = archive_upload_manager.genSql(select_sql)
        i = 0
        archive_records = []
        for data_record in data_records:
            archive_records.append({})
            archive_records[i]['dateTime'] = data_record[0]
            archive_records[i]['usUnits'] = data_record[1]
            archive_records[i]['interval'] = data_record[2]
            archive_records[i]['barometer'] = data_record[3]
            archive_records[i]['pressure'] = data_record[4]
            archive_records[i]['altimeter'] = data_record[5]
            archive_records[i]['inTemp'] = data_record[6]
            archive_records[i]['outTemp'] = data_record[7]
            archive_records[i]['inHumidity'] = data_record[8]
            archive_records[i]['outHumidity'] = data_record[9]
            archive_records[i]['windSpeed'] = data_record[10]
            archive_records[i]['windDir'] = data_record[11]
            archive_records[i]['windGust'] = data_record[12]
            archive_records[i]['windGustDir'] = data_record[13]
            archive_records[i]['rainRate'] = data_record[14]
            archive_records[i]['rain'] = data_record[15]
            archive_records[i]['dewpoint'] = data_record[16]
            archive_records[i]['windchill'] = data_record[17]
            archive_records[i]['heatindex'] = data_record[18]
            archive_records[i]['ET'] = data_record[19]
            archive_records[i]['radiation'] = data_record[20]
            archive_records[i]['UV'] = data_record[21]
            archive_records[i]['extraTemp1'] = data_record[22]
            archive_records[i]['extraTemp2'] = data_record[23]
            archive_records[i]['extraTemp3'] = data_record[24]
            archive_records[i]['soilTemp1'] = data_record[25]
            archive_records[i]['soilTemp2'] = data_record[26]
            archive_records[i]['soilTemp3'] = data_record[27]
            archive_records[i]['soilTemp4'] = data_record[28]
            archive_records[i]['leafTemp1'] = data_record[29]
            archive_records[i]['leafTemp2'] = data_record[30]
            archive_records[i]['extraHumid1'] = data_record[31]
            archive_records[i]['extraHumid2'] = data_record[32]
            archive_records[i]['soilMoist1'] = data_record[33]
            archive_records[i]['soilMoist2'] = data_record[34]
            archive_records[i]['soilMoist3'] = data_record[35]
            archive_records[i]['soilMoist4'] = data_record[36]
            archive_records[i]['leafWet1'] = data_record[37]
            archive_records[i]['leafWet2'] = data_record[38]
            archive_records[i]['rxCheckPercent'] = data_record[39]
            archive_records[i]['txBatteryStatus'] = data_record[40]
            archive_records[i]['consBatteryVoltage'] = data_record[41]
            archive_records[i]['hail'] = data_record[42]
            archive_records[i]['hailRate'] = data_record[43]
            archive_records[i]['heatingTemp'] = data_record[44]
            archive_records[i]['heatingVoltage'] = data_record[45]
            archive_records[i]['supplyVoltage'] = data_record[46]
            archive_records[i]['referenceVoltage'] = data_record[47]
            archive_records[i]['windBatteryStatus'] = data_record[48]
            archive_records[i]['rainBatteryStatus'] = data_record[49]
            archive_records[i]['outTempBatteryStatus'] = data_record[50]
            archive_records[i]['inTempBatteryStatus'] = data_record[51]
            i = i+1

        #json_data = json.dumps(archive_records)
        print("data retrieved")
        return archive_records

        #print("done")

    main()
