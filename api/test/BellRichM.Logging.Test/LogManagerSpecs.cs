using Machine.Specifications;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Serilog.Core;
using System;

using It = Machine.Specifications.It;

namespace BellRichM.Logging.Test
{
    public class LogManagerSpecs
    {
        protected static LoggingConfiguration loggingConfiguration;
        protected static LogManager logManager;

        Establish context = () =>
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("testlogging.json")
                .Build();

            loggingConfiguration = new LoggingConfiguration();
            var loggingSection = configuration.GetSection("Logging");
            new ConfigureFromConfigurationOptions<LoggingConfiguration>(loggingSection)
                .Configure(loggingConfiguration);

            logManager = new LogManager(configuration);
        };
    }

    internal class When_instantiating_a_LogManager : LogManagerSpecs
    {
        It should_set_the_console_sink_filter_switch = () =>
          logManager.LoggingFilterSwitches.ConsoleSinkFilterSwitch.ToString()
            .ShouldEqual(loggingConfiguration.FilterSwitches.ConsoleSink.Expression);

        It should_set_the_default_level_switch = () =>
          logManager.LoggingLevelSwitches.DefaultLoggingLevelSwitch.MinimumLevel
            .ShouldEqual(loggingConfiguration.LevelSwitches.Default.Level);

        It should_set_the_microsoft_level_switch = () =>
          logManager.LoggingLevelSwitches.MicrosoftLoggingLevelSwitch.MinimumLevel
            .ShouldEqual(loggingConfiguration.LevelSwitches.Microsoft.Level);

        It should_set_the_system_level_switch = () =>
          logManager.LoggingLevelSwitches.SystemLoggingLevelSwitch.MinimumLevel
            .ShouldEqual(loggingConfiguration.LevelSwitches.System.Level);

        It should_set_the_console_sink_level_switch = () =>
          logManager.LoggingLevelSwitches.ConsoleSinkLevelSwitch.MinimumLevel
            .ShouldEqual(loggingConfiguration.LevelSwitches.ConsoleSink.Level);
    }

    internal class When_creating_a_Serilog_logger : LogManagerSpecs
    {
        protected static Logger logger;

        Because of = () =>
            logger = logManager.Create();

        It should_create_the_logger = () =>
          logger.ShouldNotBeNull();

        Cleanup after = () =>
            logger.Dispose();
    }
}