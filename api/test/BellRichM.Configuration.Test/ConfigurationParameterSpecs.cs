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

#pragma warning disable 169
    Behaves_like<ConfigurationParameterBehaviors> a_production_configuration;
#pragma warning restore 169
    }

    public class When_getting_a_development_configuration : ConfigurationParameterSpecs
    {
    Establish context = () =>
    {
        environment = "Development";

        Setup();
    };

#pragma warning disable 169
    Behaves_like<ConfigurationParameterBehaviors> a_development_configuration;
#pragma warning restore 169
    }
}