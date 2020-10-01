using AutoMapper;
using BellRichM.Helpers.Test;
using BellRichM.Identity.Api.Controllers;
using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Models;
using BellRichM.Identity.Api.Repositories;
using BellRichM.Logging;
using FluentAssertions;
using Machine.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Reflection;

using It = Machine.Specifications.It;

namespace BellRichM.Identity.Api.Test.Controllers
{
    public class UsersControllerSpecs
    {
        protected static LoggingData loggingData;
        protected static Mock<ILoggerAdapter<UsersController>> loggerMock;
        protected static Mock<IMapper> mapperMock;
        protected static Mock<IUserRepository> userRepositoryMock;

        protected static UsersController usersController;

        protected static List<User> users;
        protected static List<UserModel> usersModel;

        Establish context = () =>
        {
            loggerMock = new Mock<ILoggerAdapter<UsersController>>();
            mapperMock = new Mock<IMapper>();
            userRepositoryMock = new Mock<IUserRepository>();

            users = new List<User>
            {
                new User
                {
                    Id = "id 01",
                    UserName = "user 01"
                },
                new User
                {
                    Id = "id 02",
                    UserName = "user 02"
                }
            };

            usersModel = new List<UserModel>
            {
                new UserModel
                {
                    Id = "id 01",
                    UserName = "user 01"
                },
                new UserModel
                {
                    Id = "id 02",
                    UserName = "user 02"
                }
            };

            userRepositoryMock.Setup(x => x.GetUsers()).ReturnsAsync(users);
            mapperMock.Setup(x => x.Map<List<UserModel>>(users)).Returns(usersModel);

            usersController = new UsersController(loggerMock.Object, mapperMock.Object, userRepositoryMock.Object);
        };
    }

    internal class When_users_are_found : UsersControllerSpecs
    {
        private static ObjectResult result;

        Establish context = () =>
        {
            loggingData = new LoggingData
            {
                EventLoggingData = new List<EventLoggingData>
                {
                    new EventLoggingData(
                        EventId.UsersController_Get,
                        string.Empty)
                },
                ErrorLoggingMessages = new List<string>()
            };
        };

        Because of = () =>
            result = (ObjectResult)usersController.Get().Await();

        Behaves_like<LoggingBehaviors<UsersController>> correct_logging = () => { };

        It should_return_success_status_code = () =>
            result.StatusCode.ShouldEqual(200);

        It should_return_an_object_of_type_usermodel = () =>
            result.Value.Should().BeOfType<List<UserModel>>();

        It should_return_a_usermodel = () =>
            result.Value.ShouldNotBeNull();

        It should_return_the_usermodel = () =>
        {
            var users = (List<UserModel>)result.Value;
            users.Should().BeEquivalentTo(usersModel);
        };
    }

    internal class When_decorating_Get_method : UsersControllerSpecs
    {
        private static MethodInfo methodInfo;

        Because of = () =>
            methodInfo = typeof(UsersController).GetMethod("Get");

        It should_have_CanViewUsers_policy = () =>
            methodInfo.Should()
            .BeDecoratedWith<AuthorizeAttribute>(a => a.Policy == "CanViewUsers");
    }
}