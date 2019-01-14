using BellRichM.Helpers.Test;
using BellRichM.Logging;
using Machine.Specifications;
using Moq;
using System.Collections.Generic;
using System.Linq;

using IT = Moq.It;
using It = Machine.Specifications.It;

namespace BellRichM.Logging
{
    [Behaviors]
    public class LoggingBehaviors<T>
    {
        protected static Mock<ILoggerAdapter<T>> loggerMock;

        protected static LoggingData loggingData;

        It should_log_correct_trace_messages = () =>
            loggerMock.Verify(x => x.LogDiagnosticTrace(IT.IsAny<string>(), IT.IsAny<object[]>()), Times.Never);

        It should_log_correct_debug_messages = () =>
            loggerMock.Verify(x => x.LogDiagnosticDebug(IT.IsAny<string>(), IT.IsAny<object[]>()), Times.Exactly(loggingData.DebugTimes));

        It should_log_correct_information_messages = () =>
            loggerMock.Verify(x => x.LogDiagnosticInformation(IT.IsAny<string>(), IT.IsAny<object[]>()), Times.Exactly(loggingData.InformationTimes));

        It should_log_correct_warning_messages = () =>
            loggerMock.Verify(x => x.LogDiagnosticWarning(IT.IsAny<string>(), IT.IsAny<object[]>()), Times.Never);

        It should_log_correct_critical_messages = () =>
            loggerMock.Verify(x => x.LogDiagnosticCritical(IT.IsAny<string>(), IT.IsAny<object[]>()), Times.Never);

        It should_log_correct_error_messages = () =>
        {
            foreach (var errorLoggingMessage in loggingData.ErrorLoggingMessages)
            {
                loggerMock.Verify(x => x.LogDiagnosticError(errorLoggingMessage, IT.IsAny<object[]>()), Times.Once);
            }
        };

        It should_log_correct_number_of_error_messages = () =>
            loggerMock.Verify(x => x.LogDiagnosticError(IT.IsAny<string>(), IT.IsAny<object[]>()), Times.Exactly(loggingData.ErrorLoggingMessages.Count()));

        It should_log_correct_events = () =>
        {
            foreach (var eventLoggingData in loggingData.EventLoggingData)
            {
                loggerMock.Verify(x => x.LogEvent(eventLoggingData.Id, eventLoggingData.Message, IT.IsAny<object[]>()), Times.Once);
            }
        };

        It should_log_correct_number_of_events = () =>
            loggerMock.Verify(x => x.LogEvent(IT.IsAny<EventId>(), IT.IsAny<string>(), IT.IsAny<object[]>()), Times.Exactly(loggingData.EventLoggingData.Count()));
    }
}
