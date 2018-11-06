using BellRichM.Logging;
using Machine.Specifications;
using Moq;

using IT = Moq.It;
using It = Machine.Specifications.It;

namespace BellRichM.Logging
{
    [Behaviors]
    public class LoggingBehaviors<T>
    {
        protected static Mock<ILoggerAdapter<T>> loggerMock;
        protected static int traceTimes;
        protected static int debugTimes;
        protected static int informationTimes;
        protected static int warningTimes;
        protected static int criticalTimes;
        protected static int errorTimes;
        protected static int eventTimes;

        protected It should_log_correct_trace_messages = () =>
            loggerMock.Verify(x => x.LogDiagnosticTrace(IT.IsAny<string>(), IT.IsAny<object[]>()), Times.Exactly(traceTimes));

        It should_log_correct_debug_messages = () =>
            loggerMock.Verify(x => x.LogDiagnosticDebug(IT.IsAny<string>(), IT.IsAny<object[]>()), Times.Exactly(debugTimes));

        It should_log_correct_information_messages = () =>
        {
            loggerMock.Verify(x => x.LogDiagnosticInformation(IT.IsAny<string>(), IT.IsAny<object[]>()), Times.Exactly(informationTimes));
        };

        It should_log_correct_warning_messages = () =>
            loggerMock.Verify(x => x.LogDiagnosticWarning(IT.IsAny<string>(), IT.IsAny<object[]>()), Times.Exactly(warningTimes));

        It should_log_correct_critical_messages = () =>
            loggerMock.Verify(x => x.LogDiagnosticCritical(IT.IsAny<string>(), IT.IsAny<object[]>()), Times.Exactly(criticalTimes));

        It should_log_correct_error_messages = () =>
        {
            loggerMock.Verify(x => x.LogDiagnosticError(IT.IsAny<string>(), IT.IsAny<object[]>()), Times.Exactly(errorTimes));
        };

        It should_log_correct_events = () =>
        {
            loggerMock.Verify(x => x.LogEvent(IT.IsAny<int>(), IT.IsAny<string>(), IT.IsAny<object[]>()), Times.Exactly(eventTimes));
        };
    }
}
