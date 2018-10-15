using BellRichM.Configuration;
using FluentAssertions;
using Machine.Specifications;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.IO;
using System.Linq;

using IT = Moq.It;
using It = Machine.Specifications.It;

namespace BellRichM.ConfigurationManager.Test
{
  public abstract class ConfigurationLoggingSpecs
  {
    protected static IConfiguration configuration;
    protected static string environment;
    protected static string defaultLevelSwitchValue;
    protected static string microsoftLevelSwitchValue;
    protected static string systemLevelSwitchValue;
    protected static string consoleSinkLevelSwitchValue;
    protected static string consoleSinkFilterSwitchExpression;
    Because of = () => { };

    protected static void Setup()
    {
      var configurationManager =
        new BellRichM.Configuration.ConfigurationManager(
          environment,
          Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "../../../../../src/BellRichM.Weather.Web"));
      configuration = configurationManager.Create();
    }
  }

  [Behaviors]
  public class ConfigurationLoggingBehaviors
  {
      protected static IConfiguration configuration;

      protected static string defaultLevelSwitchValue;
      protected static string microsoftLevelSwitchValue;
      protected static string systemLevelSwitchValue;
      protected static string consoleSinkLevelSwitchValue;

      protected static string consoleSinkFilterSwitchExpression;

      private static string eventLogPath = "logs";
      private static string eventLogName = "events-{Date}.log.json";
      private static string eventLogSize = "10485760";
      private static string eventLogRetention = "7";

      private static string diagnosticLogPath = "logs";
      private static string diagnosticLogName = "diagnostics-{Date}.log.json";
      private static string diagnosticLogSize = "10485760";
      private static string diagnosticLogRetention = "7";

      private static string debugLogPath = "logs";
      private static string debugLogName = "debug.log";
      private static string debugLogSize = "10485760";
      private static string debugOutputTemplate = "[{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} {Type} {RequestId}] {Message}{NewLine}";

      It should_have_a_logging_subsection = () =>
      {
        configuration.GetSection("Logging").ShouldNotBeNull();
      };

      It should_correct_number_of_logging_subsections = () =>
      {
        configuration.GetSection("Logging").GetChildren().Count().ShouldEqual(3);
      };

      It should_have_logging_level_switches = () =>
      {
        var levelSwitchConfiguration = configuration.GetSection("Logging:LevelSwitches");
        levelSwitchConfiguration.ShouldNotBeNull();
        levelSwitchConfiguration.GetChildren().Count().ShouldEqual(4);
      };

      It should_have_correctly_configured_default_level_switch = () =>
      {
        var defaultLevelSwitchConfiguration = configuration.GetSection("Logging:LevelSwitches:Default");
        defaultLevelSwitchConfiguration.ShouldNotBeNull();
        defaultLevelSwitchConfiguration["Level"].ShouldEqual(defaultLevelSwitchValue);
      };

      It should_have_correctly_configured_Microsoft_level_switch = () =>
      {
        var microsoftLevelSwitchConfiguration = configuration.GetSection("Logging:LevelSwitches:Microsoft");
        microsoftLevelSwitchConfiguration.ShouldNotBeNull();
        microsoftLevelSwitchConfiguration["Level"].ShouldEqual(microsoftLevelSwitchValue);
      };

      It should_have_correctly_configured_System_level_switch = () =>
      {
        var systemLevelSwitchConfiguration = configuration.GetSection("Logging:LevelSwitches:System");
        systemLevelSwitchConfiguration.ShouldNotBeNull();
        systemLevelSwitchConfiguration["Level"].ShouldEqual(systemLevelSwitchValue);
      };

      It should_have_correctly_configured_ConsoleSink_level_switch = () =>
      {
        var consoleSinkLevelSwitchConfiguration = configuration.GetSection("Logging:LevelSwitches:ConsoleSink");
        consoleSinkLevelSwitchConfiguration.ShouldNotBeNull();
        consoleSinkLevelSwitchConfiguration["Level"].ShouldEqual(consoleSinkLevelSwitchValue);
      };

      It should_have_logging_filter_switches = () =>
      {
        var filterSwitchConfiguration = configuration.GetSection("Logging:FilterSwitches");
        filterSwitchConfiguration.ShouldNotBeNull();
        filterSwitchConfiguration.GetChildren().Count().ShouldEqual(1);
      };

      It should_have_correctly_configured_ConsoleSink_filter_switch = () =>
      {
        var consoleSinkFilterSwitchConfiguration = configuration.GetSection("Logging:FilterSwitches:ConsoleSink");
        consoleSinkFilterSwitchConfiguration.ShouldNotBeNull();
        consoleSinkFilterSwitchConfiguration["Expression"].ShouldEqual(consoleSinkFilterSwitchExpression);
      };

      It should_have_logging_sinks = () =>
      {
        var sinksConfiguration = configuration.GetSection("Logging:Sinks");
        sinksConfiguration.ShouldNotBeNull();
        sinksConfiguration.GetChildren().Count().ShouldEqual(3);
      };

      It should_have_correctly_configured_eventLog = () =>
      {
        var eventLogConfiguration = configuration.GetSection("Logging:Sinks:EventLog");
        eventLogConfiguration.ShouldNotBeNull();
        eventLogConfiguration["LogPath"].ShouldEqual(eventLogPath);
        eventLogConfiguration["LogName"].ShouldEqual(eventLogName);
        eventLogConfiguration["LogSize"].ShouldEqual(eventLogSize);
        eventLogConfiguration["LogRetention"].ShouldEqual(eventLogRetention);
      };

      It should_have_correctly_configured_diagnosticLog = () =>
      {
        var diagnosticLogConfiguration = configuration.GetSection("Logging:Sinks:DiagnosticLog");
        diagnosticLogConfiguration.ShouldNotBeNull();
        diagnosticLogConfiguration["LogPath"].ShouldEqual(diagnosticLogPath);
        diagnosticLogConfiguration["LogName"].ShouldEqual(diagnosticLogName);
        diagnosticLogConfiguration["LogSize"].ShouldEqual(diagnosticLogSize);
        diagnosticLogConfiguration["LogRetention"].ShouldEqual(diagnosticLogRetention);
      };

      It should_have_correctly_configured_debugLog = () =>
      {
        var debugLogConfiguration = configuration.GetSection("Logging:Sinks:DebugLog");
        debugLogConfiguration.ShouldNotBeNull();
        debugLogConfiguration["LogPath"].ShouldEqual(debugLogPath);
        debugLogConfiguration["LogName"].ShouldEqual(debugLogName);
        debugLogConfiguration["LogSize"].ShouldEqual(debugLogSize);
        debugLogConfiguration["OutputTemplate"].ShouldEqual(debugOutputTemplate);
      };
  }

  public class When_getting_a_production_configuration : ConfigurationLoggingSpecs
  {
    Establish context = () =>
    {
      environment = "Production";

      defaultLevelSwitchValue = "Information";
      microsoftLevelSwitchValue = "Warning";
      systemLevelSwitchValue = "Warning";
      consoleSinkLevelSwitchValue = "Fatal";

      consoleSinkFilterSwitchExpression = "false";

      Setup();
    };

    Behaves_like<ConfigurationLoggingBehaviors> a_production_configuration;
  }

  public class When_getting_a_development_configuration : ConfigurationLoggingSpecs
  {
    Establish context = () =>
    {
      environment = "Development";

      defaultLevelSwitchValue = "Debug";
      microsoftLevelSwitchValue = "Information";
      systemLevelSwitchValue = "Information";
      consoleSinkLevelSwitchValue = "Verbose";

      consoleSinkFilterSwitchExpression = "true";

      Setup();
    };

    Behaves_like<ConfigurationLoggingBehaviors> a_development_configuration;
  }
}