using FluentAssertions;
using Machine.Specifications;
using Moq;

using IT = Moq.It;
using It = Machine.Specifications.It;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using BellRichM.Identity.Api.Controllers;
using BellRichM.Identity.Api.Models;
using BellRichM.Identity.Api.Repositories;
using BellRichM.Identity.Api.Services;

namespace BellRichM.Identity.Api.Test.Controllers
{
    internal class when_model_state_is_not_valid : UserControllerSpecs
    {
        protected const string errorCode = "errorCode";
        protected const string errorMessage = "errorMessage";
        protected static ObjectResult result;

        Establish context = () => 
            userController.ModelState.AddModelError(errorCode, errorMessage);

        Cleanup after = () =>
            userController.ModelState.Clear();

        Because of = ()  => 
            result = (ObjectResult) userController.Login(userLogin).Await();

        It should_return_correct_result_type = () =>
            result.Should().BeOfType<BadRequestObjectResult>();            

        It should_return_correct_status_code = () =>            
            result.StatusCode.ShouldEqual(400);        

        It should_return_a_SerializableError = () =>
            result.Value.Should().BeOfType<SerializableError>();

        It should_return_correct_error_code = () =>
        {
            var error = (SerializableError) result.Value;
            error.Should().ContainKey(errorCode);
        };

        It should_return_correct_error_message = () =>
        {
            var error = (SerializableError) result.Value;
            ((string[])error[errorCode]).Should().Equal(errorMessage);            
        };
    }

    internal class when_invalid_user_password_combination : UserControllerSpecs
    {
        protected static ObjectResult result;

        Establish context = () => 
            userLogin.UserName = "";

        Cleanup after = () =>
            userController.ModelState.Clear();

        Because of = ()  => 
            result = (ObjectResult) userController.Login(userLogin).Await();

        It should_return_correct_result_type = () =>
            result.Should().BeOfType<BadRequestObjectResult>();            

        It should_return_correct_status_code = () =>            
            result.StatusCode.ShouldEqual(400);        

        It should_return_a_SerializableError = () =>
            result.Value.Should().BeOfType<SerializableError>();

        It should_return_correct_error_code = () =>
        {
            var error = (SerializableError) result.Value;
            error.Should().ContainKey("loginError");
        };

        It should_return_correct_error_message = () =>
        {
            var error = (SerializableError) result.Value;
            ((string[])error["loginError"]).Should().Equal("Invalid user password combination.");            
        };
    }

    internal class when_id_and_password_is_correct : UserControllerSpecs
    {
        protected static ObjectResult result;

        Because of = ()  => 
            result = (ObjectResult) userController.Login(userLogin).Await();

        It should_return_correct_result_type = () =>
            result.Should().BeOfType<OkObjectResult>();            

        It should_return_correct_status_code = () =>            
            result.StatusCode.ShouldEqual(200);        

        It should_return_a_SerializableError = () =>
            result.Value.Should().BeOfType<AccessTokenModel>();

        It should_return_the_jwt_value = () =>
        {
            var accessToken = (AccessTokenModel)result.Value;
            accessToken.JsonWebToken.Should().Equals("jwt");            
        };
    }

    internal class UserControllerSpecs
    {
        protected static Mock<ILogger<UserController>> loggerMock;
        protected static Mock<IUserRepository> userRepositoryMock;   
        protected static Mock<IJwtManager> jwtManagerMock;
        protected static UserController userController;
        protected static UserLoginModel userLogin;
        protected const string jwt = "jwt";

        Establish context = () =>
        {
            loggerMock = new Mock<ILogger<UserController>>();
            userRepositoryMock = new Mock<IUserRepository>();
            jwtManagerMock = new Mock<IJwtManager>();            

            userLogin = new UserLoginModel
            {
                UserName = "user",
                Password = "P@ssw0rd"
            };

            jwtManagerMock.Setup(x => x.GenerateToken(userLogin.UserName, userLogin.Password)).ReturnsAsync(jwt);
            jwtManagerMock.Setup(x => x.GenerateToken("", userLogin.Password)).ReturnsAsync((string)null);

            userController = new UserController(loggerMock.Object, userRepositoryMock.Object, jwtManagerMock.Object);            
        };      
    }
}