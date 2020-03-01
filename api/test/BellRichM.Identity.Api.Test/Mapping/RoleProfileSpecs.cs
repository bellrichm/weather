using AutoMapper;
using BellRichM.Identity.Api.Mapping;
using Machine.Specifications;
using System;

using It = Machine.Specifications.It;

namespace BellRichM.Identity.Api.Test.Mapping
{
    internal class RoleProfileSpecs
    {
    }

    internal class When_creating_role_mapper : RoleProfileSpecs
    {
        private static Exception exception;

        private static MapperConfiguration mapperConfiguration;

        Establish context = () =>
            mapperConfiguration = new MapperConfiguration(c => c.AddProfile<RoleProfile>());

        Because of = () =>
            exception = Catch.Exception(() => mapperConfiguration.AssertConfigurationIsValid());

        It should_pass_validation = () =>
            exception.ShouldBeNull();
    }
}