using AutoMapper;
using BellRichM.Identity.Api.Controllers;
using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Models;
using BellRichM.Identity.Api.Repositories;
using BellRichM.Identity.Api.Services;
using FluentAssertions;
using Machine.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System.Reflection;

using IT = Moq.It;
using It = Machine.Specifications.It;

namespace BellRichM.Identity.Api.Test.Controllers
{
    internal class UserControllerSpecs
    {
      protected const string Jwt = "jwt";
      protected static Mock<ILogger<UserController>> loggerMock;
      protected static Mock<IMapper> mapperMock;
      protected static Mock<IUserRepository> userRepositoryMock;
      protected static Mock<IJwtManager> jwtManagerMock;
      protected static UserController userController;
      protected static User user;
      protected static UserLoginModel userLogin;
      protected static UserModel userModel;

      Establish context = () =>
      {
        loggerMock = new Mock<ILogger<UserController>>();
        mapperMock = new Mock<IMapper>();
        userRepositoryMock = new Mock<IUserRepository>();
        jwtManagerMock = new Mock<IJwtManager>();

        userLogin = new UserLoginModel
        {
          UserName = "user",
          Password = "P@ssw0rd"
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

        jwtManagerMock.Setup(x => x.GenerateToken(userLogin.UserName, userLogin.Password)).ReturnsAsync(Jwt);
        jwtManagerMock.Setup(x => x.GenerateToken(string.Empty, userLogin.Password)).ReturnsAsync((string)null);

        userRepositoryMock.Setup(x => x.GetById(userModel.Id)).ReturnsAsync(user);
        userRepositoryMock.Setup(x => x.GetById(string.Empty)).ReturnsAsync((User)null);

        mapperMock.Setup(x => x.Map<UserModel>(user)).Returns(userModel);

        userController = new UserController(loggerMock.Object, mapperMock.Object, userRepositoryMock.Object, jwtManagerMock.Object);
      };

      Cleanup after = () =>
        userController.Dispose();
    }

    internal class When_model_state_is_not_valid : UserControllerSpecs
    {
      protected const string ErrorCode = "errorCode";
      protected const string ErrorMessage = "errorMessage";
      protected static ObjectResult result;

      Establish context = () =>
        userController.ModelState.AddModelError(ErrorCode, ErrorMessage);

      Cleanup after = () =>
        userController.ModelState.Clear();

      Because of = () =>
        result = (ObjectResult)userController.Login(userLogin).Await();

      It should_return_correct_result_type = () =>
        result.Should().BeOfType<BadRequestObjectResult>();

      It should_return_correct_status_code = () =>
        result.StatusCode.ShouldEqual(400);

      It should_return_a_SerializableError = () =>
        result.Value.Should().BeOfType<SerializableError>();

      It should_return_correct_error_code = () =>
      {
        var error = (SerializableError)result.Value;
        error.Should().ContainKey(ErrorCode);
      };

      It should_return_correct_error_message = () =>
      {
        var error = (SerializableError)result.Value;
        ((string[])error[ErrorCode]).Should().Equal(ErrorMessage);
      };
    }

    internal class When_invalid_user_password_combination : UserControllerSpecs
    {
      protected static ObjectResult result;

      Establish context = () =>
        userLogin.UserName = string.Empty;

      Cleanup after = () =>
        userController.ModelState.Clear();

      Because of = () =>
        result = (ObjectResult)userController.Login(userLogin).Await();

      It should_return_correct_result_type = () =>
        result.Should().BeOfType<BadRequestObjectResult>();

      It should_return_correct_status_code = () =>
        result.StatusCode.ShouldEqual(400);

      It should_return_a_SerializableError = () =>
        result.Value.Should().BeOfType<SerializableError>();

      It should_return_correct_error_code = () =>
      {
        var error = (SerializableError)result.Value;
        error.Should().ContainKey("loginError");
      };

      It should_return_correct_error_message = () =>
      {
        var error = (SerializableError)result.Value;
        ((string[])error["loginError"]).Should().Equal("Invalid user password combination.");
      };
    }

    internal class When_id_and_password_is_correct : UserControllerSpecs
    {
      private static ObjectResult result;

      Because of = () =>
        result = (ObjectResult)userController.Login(userLogin).Await();

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

      Because of = () =>
        result = (ObjectResult)userController.GetById(userModel.Id).Await();

      It should_return_success_status_code = () =>
        result.StatusCode.ShouldEqual(200);

      It should_return_an_object_of_type_usermodel = () =>
        result.Value.Should().BeOfType<UserModel>();

      It should_return_a_usermodel = () =>
        result.Value.ShouldNotBeNull();

      It should_return_the_usermodel = () =>
      {
        var user = (UserModel)result.Value;
        user.ShouldBeEquivalentTo(userModel);
      };
    }

    internal class When_user_is_not_found : UserControllerSpecs
    {
      private static NotFoundResult result;

      Because of = () =>
        result = (NotFoundResult)userController.GetById(string.Empty).Await();

      It should_return_not_found_status_code = () =>
      {
        result.StatusCode.ShouldEqual(404);
      };
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
}