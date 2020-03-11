""" The uploader """
# pylint: disable=invalid-name
# pylint: enable=invalid-name
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

        # site_dict = {}
        self.archive_queue = Queue()
        self.archive_thread = RMBArchiveUploadThread(self.archive_queue,
                                                     archive_upload_manager_dict)
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
                 archiveUpload_manager_dict,
                 #host, port,
                 #user, password,
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

        self.archive_upload_manager_dict = archiveUpload_manager_dict
        self.archive_upload_DBM = None

        loginf("init RMBArchiveUploadThread")

        #self.host = host
        #self.port = to_int(port)
        #self.user = user
        #self.password = password
        #self.measurement = measurement
        #self.platform = platform
        #self.stream = stream

    def process_record(self, record, dbmanager):
        # Constructor is a different thread, so have to do this here.
        if not self.archive_upload_DBM:
            self.archive_upload_DBM = weewx.manager.open_manager(self.archive_upload_manager_dict) # pylint: disable=invalid-name

        curr_date_time = int(time.time())

        self.archive_upload_DBM.getSql('UPDATE %s SET upload_dateTime = ? WHERE dateTime= ?' %
                                       self.archive_upload_DBM.table_name,
                                       (str(curr_date_time),
                                        record["dateTime"]))


        loginf("process_record")

    def format_url(self, _):
        """Override and return the URL used to post to the WeeRT server"""

        #url = "%s %s %s" % (self.host, self.port, self.measurement)
        url = "tbd"
        return url

class RMBArchiveUploadLogin():
    """ The login class """
    def __init__(self):
        loginf("init RMBArchiveUploadLogin")

if __name__ == '__main__':
    def main():
        print("in main")

    main()