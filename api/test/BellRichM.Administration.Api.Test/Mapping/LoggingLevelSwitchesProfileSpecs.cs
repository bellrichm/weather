using AutoMapper;
using BellRichM.Administration.Api.Mapping;
using Machine.Specifications;
using System;

using It = Machine.Specifications.It;

namespace BellRichM.Administration.Api.Test.Mapping
{
    internal class LoggingLevelSwitchesProfileSpecs
    {
    }

    internal class When_creating_logging_level_switches_mapper : LoggingLevelSwitchesProfileSpecs
    {
        private static Exception exception;

        private static MapperConfiguration mapperConfiguration;

        Establish context = () =>
            mapperConfiguration = new MapperConfiguration(c => c.AddProfile<LoggingLevelSwitchesProfile>());

        Because of = () =>
            exception = Catch.Exception(() => mapperConfiguration.AssertConfigurationIsValid());

        It should_pass_validation = () =>
            exception.ShouldBeNull();
    }
}