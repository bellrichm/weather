using Destructurama.Attributed;
using FluentAssertions;
using Machine.Specifications;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

using It = Machine.Specifications.It;

namespace BellRichM.Identity.Api.Configuration
{
    public class JwtConfigurationSpecs
    {
    }

    internal class When_decorating_Audience_property : JwtConfigurationSpecs
    {
        private static PropertyInfo propertyInfo;

        Because of = () =>
            propertyInfo = typeof(JwtConfiguration).GetProperty("Audience");

        It should_have_RequiredAttribute_attribute = () =>
            propertyInfo.Should().BeDecoratedWith<RequiredAttribute>();
    }

    internal class When_decorating_Issuer_property : JwtConfigurationSpecs
    {
        private static PropertyInfo propertyInfo;

        Because of = () =>
            propertyInfo = typeof(JwtConfiguration).GetProperty("Issuer");

        It should_have_RequiredAttribute_attribute = () =>
            propertyInfo.Should().BeDecoratedWith<RequiredAttribute>();
    }

    internal class When_decorating_SecretKey_property : JwtConfigurationSpecs
    {
        private static PropertyInfo propertyInfo;

        Because of = () =>
            propertyInfo = typeof(JwtConfiguration).GetProperty("SecretKey");

        It should_have_RequiredAttribute_attribute = () =>
            propertyInfo.Should().BeDecoratedWith<RequiredAttribute>();

        It should_have_NotLoggedAttribute_attribute = () =>
            propertyInfo.Should().BeDecoratedWith<NotLoggedAttribute>();
    }
}
