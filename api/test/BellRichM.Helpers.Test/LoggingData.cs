using System.Collections.Generic;

namespace BellRichM.Helpers.Test
{
    public class LoggingData
    {
        public int DebugTimes { get; set; }
        public int InformationTimes { get; set; }
        public IEnumerable<EventLoggingData> EventLoggingData { get; set; }

        public IEnumerable<string> ErrorLoggingMessages { get; set; }
    }
}