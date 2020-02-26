using Machine.Specifications;
using Microsoft.Extensions.Configuration;
using System.IO;

using It = Machine.Specifications.It;

namespace BellRichM.ConfigurationParamenter.Test
{
    public class ConfigurationParameterSpecs
    {
        protected static IConfiguration configuration;
        protected static string environment;
        protected static string basePath = Path.Combine(System.AppDomain.CurrentDomain.BaseDirectory, "../../../../../src/BellRichM.Weather.Web");

        Because of = () => { };

        protected static void Setup()
        {
            var configurationManager =
                new BellRichM.Configuration.ConfigurationManager(
                environment,
                basePath);
            configuration = configurationManager.Create();
        }
    }

    [Behaviors]
    public class ConfigurationParameterBehaviors
    {
        protected static IConfiguration configuration;
        protected static string environment;
        protected static string basePath;

        It should_have_a_basepath_section = () =>
        {
            configuration.GetSection("BasePath").ShouldNotBeNull();
            configuration["BasePath"].ShouldEqual(basePath);
        };

        It should_have_a_environment_section = () =>
        {
            configuration.GetSection("Environment").ShouldNotBeNull();
            configuration["Environment"].ShouldEqual(environment);
        };
    }

    public class When_getting_a_production_configuration : ConfigurationParameterSpecs
    {
    Establish context = () =>
    {
        environment = "Production";

        Setup();
    };

    Behaves_like<ConfigurationParameterBehaviors> a_production_configuration = () => { };
    }

    public class When_getting_a_development_configuration : ConfigurationParameterSpecs
    {
    Establish context = () =>
    {
        environment = "Development";

        Setup();
    };

    Behaves_like<ConfigurationParameterBehaviors> a_development_configuration = () => { };
    }
}