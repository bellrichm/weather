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

namespace BellRichM.ConfigurationConnectionStrings.Test
{
    public class ConfigurationConnectionStringsSpecs
    {
        protected static IConfiguration configuration;
        protected static string environment;

        protected static string validFor;

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
    public class ConfigurationConnectionStringsBehaviors
    {
        protected static IConfiguration configuration;

        private static string identityDB = "DataSource=Data/Identity.db";

        It should_have_a_ConnectionStringssubsection = () =>
        {
            configuration.GetSection("ConnectionStrings").ShouldNotBeNull();
        };

        It should_correct_number_of_connectionStrings_subsections = () =>
        {
            configuration.GetSection("ConnectionStrings").GetChildren().Count().ShouldEqual(1);
        };

        It should_have_identityDb_section = () =>
        {
            var connectionStringsConfiguration = configuration.GetSection("ConnectionStrings");
            connectionStringsConfiguration.GetSection("(identityDb)").ShouldNotBeNull();
            connectionStringsConfiguration["(identityDb)"].ShouldEqual(identityDB);
        };
    }

    public class When_getting_a_production_configuration : ConfigurationConnectionStringsSpecs
    {
    Establish context = () =>
    {
        environment = "Production";

        Setup();
    };

#pragma warning disable 169
    Behaves_like<ConfigurationConnectionStringsBehaviors> a_production_configuration;
#pragma warning restore 169
    }

    public class When_getting_a_development_configuration : ConfigurationConnectionStringsSpecs
    {
    Establish context = () =>
    {
        environment = "Development";

        Setup();
    };

#pragma warning disable 169
    Behaves_like<ConfigurationConnectionStringsBehaviors> a_development_configuration;
#pragma warning restore 169
  }
}