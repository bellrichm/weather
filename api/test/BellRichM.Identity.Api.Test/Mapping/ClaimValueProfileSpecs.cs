using AutoMapper;
using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Mapping;
using BellRichM.Identity.Api.Models;
using FluentAssertions;
using Machine.Specifications;
using Moq;
using System;

using IT = Moq.It;
using It = Machine.Specifications.It;

#pragma warning disable SA1649 // File name should match first type name
namespace BellRichM.Identity.Api.Test.Mapping
{
    internal class When_creating_claimvalue_mapper
    {
        private static Exception exception;

        Establish context = () =>
            Mapper.Initialize(x => x.AddProfile<ClaimValueProfile>());

        Cleanup after = () =>
            AutoMapper.Mapper.Reset();

        Because of = () =>
            exception = Catch.Exception(() => Mapper.AssertConfigurationIsValid());

        It should_pass_validation = () =>
            exception.ShouldBeNull();
    }
}
#pragma warning restore SA1649 // File name should match first type name