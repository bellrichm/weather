using BellRichM.Logging;
using FluentAssertions;
using Machine.Specifications;
using Microsoft.AspNetCore.Http;
using Moq;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.TestCorrelator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using IT = Moq.It;
using It = Machine.Specifications.It;

namespace BellRichM.Logging.Test
{
    public class LoggingAdapterSpecs
    {
        protected static LoggerAdapter<LoggingAdapterSpecs> loggerAdapter;
        protected static ILogger logger;
        protected static string message = "message {int} {string} {object}";
        protected static object[] parameters;
        protected static IEnumerable<LogEvent> logEvents;
        Establish context = () =>
        {
            parameters = new object[]
            {
                1,
                "foo",
                new { intProperty = 108, stringProperty = "bar" }
            };

            logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .MinimumLevel.Verbose()
                .WriteTo.TestCorrelator()
                .CreateLogger();
            Log.Logger = logger;
            loggerAdapter = new LoggerAdapter<LoggingAdapterSpecs>();
        };
    }

    [Behaviors]
    public class LogEventBehaviors
    {
        protected static IEnumerable<LogEvent> logEvents;
        protected static object[] parameters;
        protected static string message;
        private static Dictionary<string, int> contextPropertiesCount = new Dictionary<string, int>()
        {
            { "\"TRACE\"", 2 },
            { "\"DEBUG\"", 2 },
            { "\"INFORMATION\"", 2 },
            { "\"WARNING\"", 2 },
            { "\"CRITICAL\"", 2 },
            { "\"ERROR\"", 2 },
            { "\"EVENT\"", 4 }
        };

        It should_have_correct_message = () =>
        {
            foreach (var logEvent in logEvents)
            {
                logEvent.MessageTemplate.Text.ShouldEqual(message);
            }
        };

        It should_have_correct_number_of_properties = () =>
        {
            foreach (var logEvent in logEvents)
            {
                var count = contextPropertiesCount[logEvent.Properties["Type"].ToString()];
                logEvent.Properties.Count().ShouldEqual(parameters.Length + count);
            }
        };

        It should_have_correct_SourceContext_property = () =>
        {
            foreach (var logEvent in logEvents)
            {
                logEvent.Properties["SourceContext"].ToString().ShouldEqual<string>("\"BellRichM.Logging.Test.LoggingAdapterSpecs\"");
            }
        };
    }

    public class When_logging_trace_message : LoggingAdapterSpecs
    {
        Because of = () =>
        {
            using (var testCorrelatorContext = TestCorrelator.CreateContext())
            {
                loggerAdapter.LogDiagnosticTrace(message, parameters);
                logEvents = TestCorrelator.GetLogEventsFromContextGuid(testCorrelatorContext.Guid);
            }
        };

        It should_have_one_log_event = () =>
            logEvents.Count().ShouldEqual(1);

        It should_have_correct_property = () =>
            logEvents.First()
                .Properties["Type"].ToString().ShouldEqual<string>("\"TRACE\"");

#pragma warning disable 169
        Behaves_like<LogEventBehaviors> a_event_log;
#pragma warning restore 169
    }

    public class When_logging_debug_message : LoggingAdapterSpecs
    {
        Because of = () =>
        {
            using (var testCorrelatorContext = TestCorrelator.CreateContext())
            {
                loggerAdapter.LogDiagnosticDebug(message, parameters);
                logEvents = TestCorrelator.GetLogEventsFromContextGuid(testCorrelatorContext.Guid);
            }
        };

        It should_have_one_log_event = () =>
            logEvents.Count().ShouldEqual(1);

        It should_have_correct_property = () =>
            logEvents.First()
                .Properties["Type"].ToString().ShouldEqual<string>("\"DEBUG\"");

#pragma warning disable 169
        Behaves_like<LogEventBehaviors> a_event_log;
#pragma warning restore 169

    }

    public class When_logging_informational_message : LoggingAdapterSpecs
    {
        Because of = () =>
        {
            using (var testCorrelatorContext = TestCorrelator.CreateContext())
            {
                loggerAdapter.LogDiagnosticInformation(message, parameters);
                logEvents = TestCorrelator.GetLogEventsFromContextGuid(testCorrelatorContext.Guid);
            }
        };

        It should_have_one_log_event = () =>
            logEvents.Count().ShouldEqual(1);

        It should_have_correct_property = () =>
            logEvents.First()
                .Properties["Type"].ToString().ShouldEqual<string>("\"INFORMATION\"");

#pragma warning disable 169
        Behaves_like<LogEventBehaviors> a_event_log;
#pragma warning restore 169

    }

    public class When_logging_warning_message : LoggingAdapterSpecs
    {
        Because of = () =>
        {
            using (var testCorrelatorContext = TestCorrelator.CreateContext())
            {
                loggerAdapter.LogDiagnosticWarning(message, parameters);
                logEvents = TestCorrelator.GetLogEventsFromContextGuid(testCorrelatorContext.Guid);
            }
        };

        It should_have_one_log_event = () =>
            logEvents.Count().ShouldEqual(1);

        It should_have_correct_property = () =>
            logEvents.First()
                .Properties["Type"].ToString().ShouldEqual<string>("\"WARNING\"");

#pragma warning disable 169
        Behaves_like<LogEventBehaviors> a_event_log;
#pragma warning restore 169
    }

    public class When_logging_critical_message : LoggingAdapterSpecs
    {
        Because of = () =>
        {
            using (var testCorrelatorContext = TestCorrelator.CreateContext())
            {
                loggerAdapter.LogDiagnosticCritical(message, parameters);
                logEvents = TestCorrelator.GetLogEventsFromContextGuid(testCorrelatorContext.Guid);
            }
        };

        It should_have_one_log_event = () =>
            logEvents.Count().ShouldEqual(1);

        It should_have_correct_property = () =>
            logEvents.First()
                .Properties["Type"].ToString().ShouldEqual<string>("\"CRITICAL\"");

#pragma warning disable 169
        Behaves_like<LogEventBehaviors> a_event_log;
#pragma warning restore 169
    }

    public class When_logging_error_message : LoggingAdapterSpecs
    {
        Because of = () =>
        {
            using (var testCorrelatorContext = TestCorrelator.CreateContext())
            {
                loggerAdapter.LogDiagnosticError(message, parameters);
                logEvents = TestCorrelator.GetLogEventsFromContextGuid(testCorrelatorContext.Guid);
            }
        };

        It should_have_one_log_event = () =>
            logEvents.Count().ShouldEqual(1);

        It should_have_correct_property = () =>
        {
            var logEvent = logEvents.First();
            logEvent.Properties["Type"].ToString().ShouldEqual<string>("\"ERROR\"");
        };

#pragma warning disable 169
        Behaves_like<LogEventBehaviors> a_event_log;
#pragma warning restore 169
    }

    public class When_logging_event_message : LoggingAdapterSpecs
    {
        Because of = () =>
        {
            using (var testCorrelatorContext = TestCorrelator.CreateContext())
            {
                loggerAdapter.LogEvent(EventId.EndRequest, message, parameters);
                logEvents = TestCorrelator.GetLogEventsFromContextGuid(testCorrelatorContext.Guid);
            }
        };

        It should_have_two_log_event = () =>
            logEvents.Count().ShouldEqual(2);

        It should_have_one_event_type_property = () =>
            logEvents.Where(logEvent => logEvent.Properties["Type"].ToString() == "\"EVENT\"")
                .Count().ShouldEqual(1);

        It should_have_correct_id = () =>
        {
            var id = logEvents.Where(logEvent => logEvent.Properties["Type"].ToString() == "\"EVENT\"")
                .First()
                .Properties["Id"]
                .ToString();
            id.ShouldEqual(EventId.EndRequest.ToString("D"));
        };

        It should_have_correct_event = () =>
        {
            var eventName = logEvents.Where(logEvent => logEvent.Properties["Type"].ToString() == "\"EVENT\"")
                .First()
                .Properties["Event"]
                .ToString();
            eventName.ShouldEqual(EventId.EndRequest.ToString());
        };

        It should_have_one_information_type_property = () =>
            logEvents.Where(logEvent => logEvent.Properties["Type"].ToString() == "\"INFORMATION\"")
                .Count().ShouldEqual(1);

#pragma warning disable 169
        Behaves_like<LogEventBehaviors> a_event_log;
#pragma warning restore 169

    }
}