using AutoMapper;
using BellRichM.Administration.Api.Mapping;
using BellRichM.Administration.Api.Models;
using FluentAssertions;
using Machine.Specifications;
using Moq;
using System;

using IT = Moq.It;
using It = Machine.Specifications.It;

#pragma warning disable SA1649 // File name should match first type name
namespace BellRichM.Administration.Api.Test.Mapping
{
    internal class When_creating_logging_level_switches_mapper
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
#pragma warning restore SA1649 // File name should match first type name