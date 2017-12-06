using FluentAssertions;
using Machine.Specifications;
using Moq;

using IT = Moq.It;
using It = Machine.Specifications.It;

using System;
using AutoMapper;
using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Mapping;
using BellRichM.Identity.Api.Models;

namespace BellRichM.Identity.Api.Test.Mapping
{
    internal class when_creating_user_mapper
    {
        private static Exception exception;

        Establish Context = () =>
            Mapper.Initialize(x => x.AddProfile<UserProfile>());

        Cleanup after = () =>
            AutoMapper.Mapper.Reset();    

        Because of = ()  =>
            exception = Catch.Exception(() => Mapper.AssertConfigurationIsValid());

        It should_pass_validation = () =>
            exception.ShouldBeNull();
    }
}