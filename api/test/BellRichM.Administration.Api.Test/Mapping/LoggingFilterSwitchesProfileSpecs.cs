using AutoMapper;
using BellRichM.Administration.Api.Mapping;
using Machine.Specifications;
using System;

using It = Machine.Specifications.It;

namespace BellRichM.Administration.Api.Test.Mapping
{
    internal class LoggingFilterSwitchesProfileSpecs
    {
    }

    internal class When_creating_logging_filter_switches_mapper : LoggingFilterSwitchesProfileSpecs
    {
        private static Exception exception;

        private static MapperConfiguration mapperConfiguration;

        Establish context = () =>
            mapperConfiguration = new MapperConfiguration(c => c.AddProfile<LoggingFilterSwitchesProfile>());

        Because of = () =>
            exception = Catch.Exception(() => mapperConfiguration.AssertConfigurationIsValid());

        It should_pass_validation = () =>
            exception.ShouldBeNull();
    }
}