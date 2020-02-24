using AutoMapper;
using BellRichM.Weather.Api.Mapping;
using Machine.Specifications;
using System;

using It = Machine.Specifications.It;

#pragma warning disable SA1649 // File name should match first type name
namespace BellRichM.Weather.Api.Test.Mapping
{
    internal class When_creating_condition_mapper
    {
        private static Exception exception;

        private static MapperConfiguration mapperConfiguration;

        Establish context = () =>
            mapperConfiguration = new MapperConfiguration(c => c.AddProfile<ConditionProfile>());

        Because of = () =>
            exception = Catch.Exception(() => mapperConfiguration.AssertConfigurationIsValid());

        It should_pass_validation = () =>
            exception.ShouldBeNull();
    }
}
#pragma warning restore SA1649 // File name should match first type name