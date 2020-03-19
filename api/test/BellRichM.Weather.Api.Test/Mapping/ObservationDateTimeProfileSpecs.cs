using AutoMapper;
using BellRichM.Weather.Api.Mapping;
using Machine.Specifications;
using System;

namespace BellRichM.Weather.Api.Test.Mapping
{
    public class ObservationDateTimeProfileSpecs
    {
    }

    internal class When_creating_observation_datetime_mapper : ObservationDateTimeProfileSpecs
    {
        private static Exception exception;

        private static MapperConfiguration mapperConfiguration;

        Establish context = () =>
            mapperConfiguration = new MapperConfiguration(c => c.AddProfile<ObservationDateTimeProfile>());

        Because of = () =>
            exception = Catch.Exception(() => mapperConfiguration.AssertConfigurationIsValid());

        It should_pass_validation = () =>
            exception.ShouldBeNull();
    }
}