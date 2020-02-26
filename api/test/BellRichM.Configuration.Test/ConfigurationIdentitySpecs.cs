using Machine.Specifications;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Linq;

using It = Machine.Specifications.It;

namespace BellRichM.ConfigurationIdentity.Test
{
  public abstract class ConfigurationIdentitySpecs
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
  public class ConfigurationIdentityBehaviors
  {
    protected static IConfiguration configuration;

    protected static string validFor;

    private static string audience = "audience";
    private static string issuer = "issuer";
    private static string secretKey = string.Empty;

    It should_have_a_Identitysubsection = () =>
    {
        configuration.GetSection("Identity").ShouldNotBeNull();
    };

    It should_correct_number_of_identity_subsections = () =>
    {
        configuration.GetSection("Identity").GetChildren().Count().ShouldEqual(2);
    };

    It should_have_jwt_configuration = () =>
    {
        var jwtConfiguration = configuration.GetSection("Identity:JwtConfiguration");
        jwtConfiguration.ShouldNotBeNull();
        jwtConfiguration.GetChildren().Count().ShouldEqual(3);
    };

    It should_have_correctly_configured_issuer = () =>
      {
        var jwtConfiguration = configuration.GetSection("Identity:JwtConfiguration");
        jwtConfiguration.ShouldNotBeNull();
        jwtConfiguration["Issuer"].ShouldEqual(issuer);
      };

    It should_have_correctly_configured_audience = () =>
      {
        var jwtConfiguration = configuration.GetSection("Identity:JwtConfiguration");
        jwtConfiguration.ShouldNotBeNull();
        jwtConfiguration["Audience"].ShouldEqual(audience);
      };

    It should_have_correctly_configured_validFor = () =>
      {
        var jwtConfiguration = configuration.GetSection("Identity:JwtConfiguration");
        jwtConfiguration.ShouldNotBeNull();
        jwtConfiguration["ValidFor"].ShouldEqual(validFor);
      };

    It should_have_secretKey_section = () =>
    {
        var identityConfiguration = configuration.GetSection("Identity");
        identityConfiguration.GetSection("SecretKey").ShouldNotBeNull();

        identityConfiguration["SecretKey"].ShouldEqual(secretKey);
    };
  }

  public class When_getting_a_production_configuration : ConfigurationIdentitySpecs
  {
    Establish context = () =>
    {
      environment = "Production";
      validFor = "5";

      Setup();
    };

    Behaves_like<ConfigurationIdentityBehaviors> a_production_configuration = () => { };
  }

  public class When_getting_a_development_configuration : ConfigurationIdentitySpecs
  {
    Establish context = () =>
    {
      environment = "Development";
      validFor = "1440";

      Setup();
    };

    Behaves_like<ConfigurationIdentityBehaviors> a_development_configuration = () => { };
  }
}