using AutoMapper;
using BellRichM.Api.Models;
using BellRichM.Helpers.Test;
using BellRichM.Identity.Api.Controllers;
using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Exceptions;
using BellRichM.Identity.Api.Models;
using BellRichM.Identity.Api.Repositories;
using BellRichM.Identity.Api.Services;
using BellRichM.Logging;
using FluentAssertions;
using Machine.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Reflection;

using IT = Moq.It;
using It = Machine.Specifications.It;

namespace BellRichM.Identity.Api.Test.Controllers
{
    internal class UserControllerSpecs
    {
        protected const string Jwt = "jwt";
        protected const string UserName = "userName";
        protected const string Password = "P@ssw0rd";
        protected static LoggingData loggingData;
        protected static Mock<ILoggerAdapter<UserController>> loggerMock;
        protected static Mock<IMapper> mapperMock;
        protected static Mock<IUserRepository> userRepositoryMock;
        protected static Mock<IJwtManager> jwtManagerMock;
        protected static UserController userController;
        protected static User user;
        protected static UserLoginModel userLogin;
        protected static UserCreateModel userCreate;
        protected static UserModel userModel;
        protected static CreateUserException createUserException;
        protected static DeleteUserException deleteUserException;

        Establish context = () =>
        {
            loggerMock = new Mock<ILoggerAdapter<UserController>>();
            mapperMock = new Mock<IMapper>();
            userRepositoryMock = new Mock<IUserRepository>();
            jwtManagerMock = new Mock<IJwtManager>();

            userLogin = new UserLoginModel
            {
                UserName = "user",
                Password = "P@ssw0rd"
            };

            userCreate = new UserCreateModel
            {
                UserName = UserName,
            };

            user = new User
            {
                Id = "id",
                UserName = "userName",
            };

            userModel = new UserModel
            {
                Id = "id",
                UserName = "userName",
            };

            createUserException = new CreateUserException(CreateUserExceptionCode.CreateUserFailed);
            deleteUserException = new DeleteUserException(DeleteUserExceptionCode.DeleteUserFailed);

            jwtManagerMock.Setup(x => x.GenerateToken(userLogin.UserName, userLogin.Password)).ReturnsAsync(Jwt);
            jwtManagerMock.Setup(x => x.GenerateToken(string.Empty, userLogin.Password)).ReturnsAsync((string)null);

            userRepositoryMock.Setup(x => x.GetById(userModel.Id)).ReturnsAsync(user);
            userRepositoryMock.Setup(x => x.GetById(string.Empty)).ReturnsAsync((User)null);

            userRepositoryMock.Setup(x => x.Create(IT.IsAny<User>(), string.Empty))
                .Throws(createUserException);

            userRepositoryMock.Setup(x => x.Delete(string.Empty))
                .Throws(deleteUserException);

            mapperMock.Setup(x => x.Map<UserModel>(user)).Returns(userModel);

            userController = new UserController(loggerMock.Object, mapperMock.Object, userRepositoryMock.Object, jwtManagerMock.Object);
            userController.ControllerContext.HttpContext = new DefaultHttpContext();
            userController.ControllerContext.HttpContext.TraceIdentifier = "traceIdentifier";
        };

        Cleanup after = () =>
            userController.Dispose();
    }

    internal class When_UserLoginModel_state_is_not_valid : UserControllerSpecs
    {
        protected const string ErrorCode = "errorCode";
        protected const string ErrorMessage = "errorMessage";
        protected static ObjectResult result;

        Establish context = () =>
        {
            loggingData = new LoggingData
            {
                InformationTimes = 1,
                EventLoggingData = new List<EventLoggingData>
                {
                    new EventLoggingData(
                        EventId.UserController_Login,
                        "{@userLogin}")
                },
                ErrorLoggingMessages = new List<string>()
            };

            userController.ModelState.AddModelError(ErrorCode, ErrorMessage);
        };

        Cleanup after = () =>
            userController.ModelState.Clear();

        Because of = () =>
            result = (ObjectResult)userController.Login(userLogin).Await();

#pragma warning disable 169
        Behaves_like<LoggingBehaviors<UserController>> correct_logging;
#pragma warning restore 169

        It should_return_correct_result_type = () =>
            result.Should().BeOfType<BadRequestObjectResult>();

        It should_return_correct_status_code = () =>
            result.StatusCode.ShouldEqual(400);

        It should_return_a_ErrorResponseModel = () =>
            result.Value.Should().BeOfType<ErrorResponseModel>();
    }

    internal class When_invalid_user_password_combination : UserControllerSpecs
    {
        protected static ObjectResult result;

        Establish context = () =>
        {
            loggingData = new LoggingData
            {
                InformationTimes = 1,
                EventLoggingData = new List<EventLoggingData>
                {
                    new EventLoggingData(
                        EventId.UserController_Login,
                        "{@userLogin}")
                },
                ErrorLoggingMessages = new List<string>()
            };

            userLogin.UserName = string.Empty;
        };

        Cleanup after = () =>
            userController.ModelState.Clear();

        Because of = () =>
            result = (ObjectResult)userController.Login(userLogin).Await();

#pragma warning disable 169
        Behaves_like<LoggingBehaviors<UserController>> correct_logging;
#pragma warning restore 169

        It should_return_correct_result_type = () =>
            result.Should().BeOfType<BadRequestObjectResult>();

        It should_return_correct_status_code = () =>
            result.StatusCode.ShouldEqual(400);

        It should_return_a_ErrorResponseModel = () =>
            result.Value.Should().BeOfType<ErrorResponseModel>();
    }

    internal class When_id_and_password_is_correct : UserControllerSpecs
    {
        private static ObjectResult result;

        Establish context = () =>
        {
            loggingData = new LoggingData
            {
                EventLoggingData = new List<EventLoggingData>
                {
                    new EventLoggingData(
                        EventId.UserController_Login,
                        "{@userLogin}")
                },
                ErrorLoggingMessages = new List<string>()
            };
        };

        Because of = () =>
            result = (ObjectResult)userController.Login(userLogin).Await();

#pragma warning disable 169
        Behaves_like<LoggingBehaviors<UserController>> correct_logging;
#pragma warning restore 169

        It should_return_correct_result_type = () =>
            result.Should().BeOfType<OkObjectResult>();

        It should_return_correct_status_code = () =>
            result.StatusCode.ShouldEqual(200);

        It should_return_a_SerializableError = () =>
            result.Value.Should().BeOfType<AccessTokenModel>();

        It should_return_the_jwt_value = () =>
        {
            var accessToken = (AccessTokenModel)result.Value;
            accessToken.JsonWebToken.ShouldEqual("jwt");
        };
    }

    internal class When_user_is_found : UserControllerSpecs
    {
        private static ObjectResult result;

        Establish context = () =>
        {
            loggingData = new LoggingData
            {
                EventLoggingData = new List<EventLoggingData>
                {
                    new EventLoggingData(
                        EventId.UserController_GetById,
                        "{@id}")
                },
                ErrorLoggingMessages = new List<string>()
            };
        };

        Because of = () =>
            result = (ObjectResult)userController.GetById(userModel.Id).Await();

#pragma warning disable 169
        Behaves_like<LoggingBehaviors<UserController>> correct_logging;
#pragma warning restore 169

        It should_return_success_status_code = () =>
            result.StatusCode.ShouldEqual(200);

        It should_return_an_object_of_type_usermodel = () =>
            result.Value.Should().BeOfType<UserModel>();

        It should_return_a_usermodel = () =>
            result.Value.ShouldNotBeNull();

        It should_return_the_usermodel = () =>
        {
            var user = (UserModel)result.Value;
            user.Should().BeEquivalentTo(userModel);
        };
    }

    internal class When_user_is_not_found : UserControllerSpecs
    {
        private static NotFoundResult result;

        Establish context = () =>
        {
            loggingData = new LoggingData
            {
                EventLoggingData = new List<EventLoggingData>
                {
                    new EventLoggingData(
                        EventId.UserController_GetById,
                        "{@id}")
                },
                ErrorLoggingMessages = new List<string>()
            };
        };

        Because of = () =>
            result = (NotFoundResult)userController.GetById(string.Empty).Await();

#pragma warning disable 169
        Behaves_like<LoggingBehaviors<UserController>> correct_logging;
#pragma warning restore 169

        It should_return_not_found_status_code = () =>
            result.StatusCode.ShouldEqual(404);
    }

    internal class When_decorating_GetById_method : UserControllerSpecs
    {
        private static MethodInfo methodInfo;

        Because of = () =>
            methodInfo = typeof(UserController).GetMethod("GetById");

        It should_have_CanViewUsers_policy = () =>
            methodInfo.Should()
            .BeDecoratedWith<AuthorizeAttribute>(a => a.Policy == "CanViewUsers");
    }

    internal class When_UserCreateModel_is_not_valid : UserControllerSpecs
    {
        protected const string ErrorCode = "errorCode";
        protected const string ErrorMessage = "errorMessage";
        protected static ObjectResult result;

        Establish context = () =>
        {
            loggingData = new LoggingData
            {
                InformationTimes = 1,
                EventLoggingData = new List<EventLoggingData>
                {
                    new EventLoggingData(
                        EventId.UserController_Create,
                        "{@userCreate}")
                },
                ErrorLoggingMessages = new List<string>()
            };

            userController.ModelState.AddModelError(ErrorCode, ErrorMessage);
        };

        Cleanup after = () =>
            userController.ModelState.Clear();

        Because of = () =>
            result = (ObjectResult)userController.Create(userCreate).Await();

#pragma warning disable 169
        Behaves_like<LoggingBehaviors<UserController>> correct_logging;
#pragma warning restore 169

        It should_return_correct_result_type = () =>
            result.Should().BeOfType<BadRequestObjectResult>();

        It should_return_correct_status_code = () =>
            result.StatusCode.ShouldEqual(400);

        It should_return_a_ErrorResponseModel = () =>
            result.Value.Should().BeOfType<ErrorResponseModel>();
    }

    internal class When_creating_user_fails : UserControllerSpecs
    {
        protected static ObjectResult result;

        Establish context = () =>
        {
            loggingData = new LoggingData
            {
                InformationTimes = 1,
                EventLoggingData = new List<EventLoggingData>
                {
                    new EventLoggingData(
                        EventId.UserController_Create,
                        "{@userCreate}")
                },
                ErrorLoggingMessages = new List<string>()
            };

            userCreate.Password = string.Empty;
        };

        Cleanup after = () =>
            userController.ModelState.Clear();

        Because of = () =>
            result = (ObjectResult)userController.Create(userCreate).Await();

#pragma warning disable 169
        Behaves_like<LoggingBehaviors<UserController>> correct_logging;
#pragma warning restore 169

        It should_return_correct_result_type = () =>
            result.Should().BeOfType<BadRequestObjectResult>();

        It should_return_correct_status_code = () =>
            result.StatusCode.ShouldEqual(400);

        It should_return_a_ErrorResponseModel = () =>
            result.Value.Should().BeOfType<ErrorResponseModel>();
    }

    internal class When_creating_user_succeeds : UserControllerSpecs
    {
        protected static ObjectResult result;

        Establish context = () =>
        {
            loggingData = new LoggingData
            {
                EventLoggingData = new List<EventLoggingData>
                {
                    new EventLoggingData(
                        EventId.UserController_Create,
                        "{@userCreate}")
                },
                ErrorLoggingMessages = new List<string>()
            };

            userCreate.UserName = UserName;
            userCreate.Password = Password;
            userRepositoryMock.Setup(x => x.Create(IT.IsAny<User>(), userCreate.Password)).ReturnsAsync(user);
        };

        Because of = () =>
            result = (ObjectResult)userController.Create(userCreate).Await();

#pragma warning disable 169
        Behaves_like<LoggingBehaviors<UserController>> correct_logging;
#pragma warning restore 169

        It should_return_success_status_code = () =>
            result.StatusCode.ShouldEqual(200);

        It should_return_an_object_of_type_usermodel = () =>
            result.Value.Should().BeOfType<UserModel>();

        It should_return_a_usermodel = () =>
            result.Value.ShouldNotBeNull();

        It should_return_the_usermodel = () =>
        {
            var user = (UserModel)result.Value;
            user.Should().Equals(userModel);
        };
    }

    internal class When_decorating_Create_method : UserControllerSpecs
    {
        protected static MethodInfo methodInfo;

        Because of = () =>
            methodInfo = typeof(UserController).GetMethod("Create");

        It should_have_CanViewUsers_policy = () =>
            methodInfo.Should()
            .BeDecoratedWith<AuthorizeAttribute>(a => a.Policy == "CanCreateUsers");
    }

    internal class When_deleting_user_fails : UserControllerSpecs
    {
        protected static ObjectResult result;

        // Establish context = () =>
        // userCreate.Password = string.Empty;
        Cleanup after = () =>
            userController.ModelState.Clear();

        Establish context = () =>
        {
            loggingData = new LoggingData
            {
                InformationTimes = 1,
                EventLoggingData = new List<EventLoggingData>
                {
                    new EventLoggingData(
                        EventId.UserController_Delete,
                        "{@id}")
                },
                ErrorLoggingMessages = new List<string>()
            };
        };

        Because of = () =>
        {
            result = (ObjectResult)userController.Delete(string.Empty).Await();
        };

#pragma warning disable 169
        Behaves_like<LoggingBehaviors<UserController>> correct_logging;
#pragma warning restore 169

        It should_return_correct_result_type = () =>
            result.Should().BeOfType<BadRequestObjectResult>();

        It should_return_correct_status_code = () =>
            result.StatusCode.ShouldEqual(400);

        It should_return_a_ErrorResponseModel = () =>
            result.Value.Should().BeOfType<ErrorResponseModel>();
    }

    internal class When_deleting_user_succeeds : UserControllerSpecs
    {
        protected static NoContentResult result;

        Establish context = () =>
        {
            loggingData = new LoggingData
            {
                EventLoggingData = new List<EventLoggingData>
                {
                    new EventLoggingData(
                        EventId.UserController_Delete,
                        "{@id}")
                },
                ErrorLoggingMessages = new List<string>()
            };
        };

        Because of = () =>
            result = (NoContentResult)userController.Delete("id").Await();

#pragma warning disable 169
        Behaves_like<LoggingBehaviors<UserController>> correct_logging;
#pragma warning restore 169

        It should_return_success_status_code = () =>
            result.StatusCode.ShouldEqual(204);
    }

    internal class When_decorating_Delete_method : UserControllerSpecs
    {
        protected static MethodInfo methodInfo;

        Because of = () =>
            methodInfo = typeof(UserController).GetMethod("Delete");

        It should_have_CanViewUsers_policy = () =>
            methodInfo.Should()
            .BeDecoratedWith<AuthorizeAttribute>(a => a.Policy == "CanDeleteUsers");
    }
}