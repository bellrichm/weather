using AutoMapper;
using BellRichM.Identity.Api.Mapping;
using Machine.Specifications;
using System;

using It = Machine.Specifications.It;

namespace BellRichM.Identity.Api.Test.Mapping
{
    internal class ClaimValueProfileSpecs
    {
    }

    internal class When_creating_claimvalue_mapper : ClaimValueProfileSpecs
    {
        private static Exception exception;

        private static MapperConfiguration mapperConfiguration;

        Establish context = () =>
            mapperConfiguration = new MapperConfiguration(c => c.AddProfile<ClaimValueProfile>());

        Because of = () =>
            exception = Catch.Exception(() => mapperConfiguration.AssertConfigurationIsValid());

        It should_pass_validation = () =>
            exception.ShouldBeNull();
    }
}