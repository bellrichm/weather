using AutoMapper;
using BellRichM.Identity.Api.Mapping;
using Machine.Specifications;
using System;

using It = Machine.Specifications.It;

namespace BellRichM.Identity.Api.Test.Mapping
{
    internal class UserProfileSpecs
    {
    }

    internal class When_creating_user_mapper : UserProfileSpecs
    {
        private static Exception exception;

        private static MapperConfiguration mapperConfiguration;

        Establish context = () =>
            mapperConfiguration = new MapperConfiguration(c => c.AddProfile<UserProfile>());

        Because of = () =>
            exception = Catch.Exception(() => mapperConfiguration.AssertConfigurationIsValid());

        It should_pass_validation = () =>
            exception.ShouldBeNull();
    }
}