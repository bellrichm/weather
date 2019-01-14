using BellRichM.Logging;

namespace BellRichM.Helpers.Test
{
    public class EventLoggingData
    {
        public EventLoggingData(EventId id, string message)
        {
            Id = id;
            Message = message;
        }

        public EventId Id { get; }

        public string Message { get; }
    }
}