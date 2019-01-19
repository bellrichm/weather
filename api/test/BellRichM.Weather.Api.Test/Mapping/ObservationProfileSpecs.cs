using AutoMapper;
using BellRichM.Weather.Api.Mapping;
using FluentAssertions;
using Machine.Specifications;
using Moq;
using System;

using IT = Moq.It;
using It = Machine.Specifications.It;

#pragma warning disable SA1649 // File name should match first type name
namespace BellRichM.Weather.Api.Test.Mapping
{
    internal class When_creating_observation_mapper
    {
        private static Exception exception;

        Establish context = () =>
            Mapper.Initialize(x => x.AddProfile<ObservationProfile>());

        Cleanup after = () =>
            AutoMapper.Mapper.Reset();

        Because of = () =>
            exception = Catch.Exception(() => Mapper.AssertConfigurationIsValid());

        It should_pass_validation = () =>
            exception.ShouldBeNull();
    }
}
#pragma warning restore SA1649 // File name should match first type name