using System.Reflection;
using AutoMapper;
using BellRichM.Identity.Api.Controllers;
using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Exceptions;
using BellRichM.Identity.Api.Models;
using BellRichM.Identity.Api.Repositories;
using BellRichM.Logging;
using FluentAssertions;
using Machine.Specifications;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;

using IT = Moq.It;
using It = Machine.Specifications.It;

namespace BellRichM.Identity.Api.Test.Controllers
{
    internal class RoleControllerSpecs
    {
        protected const string RoleId = "roleId";
        protected const string RoleName = "roleName";
        protected const string RoleIdNotFound = "";
        protected const string ErrorCode = "errorCode";
        protected const string ErrorMessage = "errorMessage";

        protected static CreateRoleException createRoleException;
        protected static DeleteRoleException deleteRoleException;

        protected static RoleController roleController;

        protected static Mock<ILoggerAdapter<RoleController>> loggerMock;
        protected static Mock<IMapper> mapperMock;
        protected static Mock<IRoleRepository> roleRepositoryMock;

        protected static Role role;
        protected static RoleModel roleModel;

        Establish context = () =>
        {
            loggerMock = new Mock<ILoggerAdapter<RoleController>>();
            mapperMock = new Mock<IMapper>();
            roleRepositoryMock = new Mock<IRoleRepository>();

            role = new Role
            {
                Id = RoleId,
                Name = RoleName
            };

            roleModel = new RoleModel
            {
                Id = RoleId,
                Name = RoleName
            };

            createRoleException = new CreateRoleException(CreateRoleExceptionCode.CreateRoleFailed);
            deleteRoleException = new DeleteRoleException(DeleteRoleExceptionCode.DeleteRoleFailed);

            mapperMock.Setup(x => x.Map<RoleModel>(role)).Returns(roleModel);
            mapperMock.Setup(x => x.Map<Role>(roleModel)).Returns(role);

            roleRepositoryMock.Setup(x => x.GetById(RoleId)).ReturnsAsync(role);
            roleRepositoryMock.Setup(x => x.GetById(RoleIdNotFound)).ReturnsAsync((Role)null);

            roleRepositoryMock.Setup(x => x.Create(null)).Throws(createRoleException);
            roleRepositoryMock.Setup(x => x.Create(role)).ReturnsAsync(role);

            roleRepositoryMock.Setup(x => x.Delete(RoleIdNotFound)).Throws(deleteRoleException);

            roleController = new RoleController(loggerMock.Object, mapperMock.Object, roleRepositoryMock.Object);
        };

        Cleanup after = () =>
            roleController.Dispose();
    }

    internal class When_role_is_found : RoleControllerSpecs
    {
        private static ObjectResult result;

        Because of = () =>
            result = (ObjectResult)roleController.GetById(RoleId).Await();

        It should_return_success_status_code = () =>
            result.StatusCode.ShouldEqual(200);

        It should_return_a_rolemodel = () =>
            result.Value.ShouldNotBeNull();

        It should_return_an_object_of_type_RoleModel = () =>
            result.Value.Should().BeOfType<RoleModel>();

        It should_return_the_rolemodel = () =>
        {
            var role = (RoleModel)result.Value;
            role.Should().Equals(roleModel);
        };
    }

    internal class When_role_is_not_found : RoleControllerSpecs
    {
        private static NotFoundResult result;

        Because of = () =>
            result = (NotFoundResult)roleController.GetById(RoleIdNotFound).Await();

        It should_return_not_found_status_code = () =>
            result.StatusCode.ShouldEqual(404);
    }

    internal class When_decorating_Role_GetById_method : RoleControllerSpecs
    {
        private static MethodInfo methodInfo;

        Because of = () =>
            methodInfo = typeof(RoleController).GetMethod("GetById");

        It should_have_CanViewRoles_policy = () =>
            methodInfo.Should()
            .BeDecoratedWith<AuthorizeAttribute>(a => a.Policy == "CanViewRoles");
    }

    internal class When_RoleModel_is_not_valid : RoleControllerSpecs
    {
        private static ObjectResult result;

        Establish context = () =>
            roleController.ModelState.AddModelError(ErrorCode, ErrorMessage);

        Cleanup after = () =>
            roleController.ModelState.Clear();

        Because of = () =>
            result = (ObjectResult)roleController.Create(roleModel).Await();

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

    internal class When_creating_role_fails : RoleControllerSpecs
    {
        private static ObjectResult result;

        Cleanup after = () =>
            roleController.ModelState.Clear();

        Because of = () =>
            result = (ObjectResult)roleController.Create(null).Await();

        It should_return_correct_result_type = () =>
            result.Should().BeOfType<BadRequestObjectResult>();

        It should_return_correct_status_code = () =>
            result.StatusCode.ShouldEqual(400);

        It should_return_a_SerializableError = () =>
            result.Value.Should().BeOfType<SerializableError>();

        It should_return_correct_error_code = () =>
        {
            var error = (SerializableError)result.Value;
            error.Should().ContainKey(createRoleException.Code);
        };

        It should_return_correct_error_message = () =>
        {
            var error = (SerializableError)result.Value;
            ((string[])error[createRoleException.Code]).Should().Equal(createRoleException.Message);
        };
    }

    internal class When_creating_role_succeeds : RoleControllerSpecs
    {
        private static ObjectResult result;

        Because of = () =>
            result = (ObjectResult)roleController.Create(roleModel).Await();

        It should_return_success_status_code = () =>
            result.StatusCode.ShouldEqual(200);

        It should_return_an_object_of_type_RoleModel = () =>
            result.Value.Should().BeOfType<RoleModel>();

        It should_return_a_rolemodel = () =>
            result.Value.ShouldNotBeNull();

        It should_return_the_rolemodel = () =>
        {
            var user = (RoleModel)result.Value;
            user.Should().Equals(roleModel);
        };
    }

    internal class When_decorating_Role_Create_method : RoleControllerSpecs
    {
        private static MethodInfo methodInfo;

        Because of = () =>
        {
            methodInfo = typeof(RoleController).GetMethod("Create");
        };

        It should_have_CanCreateRoles_policy = () =>
            methodInfo.Should()
            .BeDecoratedWith<AuthorizeAttribute>(a => a.Policy == "CanCreateRoles");
    }

    internal class When_deleting_role_fails : RoleControllerSpecs
    {
        private static ObjectResult result;

        Cleanup after = () =>
            roleController.ModelState.Clear();

        Because of = () =>
            result = (ObjectResult)roleController.Delete(RoleIdNotFound).Await();

        It should_return_correct_result_type = () =>
            result.Should().BeOfType<BadRequestObjectResult>();

        It should_return_correct_status_code = () =>
            result.StatusCode.ShouldEqual(400);

        It should_return_a_SerializableError = () =>
            result.Value.Should().BeOfType<SerializableError>();

        It should_return_correct_error_code = () =>
        {
            var error = (SerializableError)result.Value;
            error.Should().ContainKey(deleteRoleException.Code);
        };

        It should_return_correct_error_message = () =>
        {
            var error = (SerializableError)result.Value;
            ((string[])error[deleteRoleException.Code]).Should().Equal(deleteRoleException.Message);
        };
    }

    internal class When_deleting_role_succeeds : RoleControllerSpecs
    {
        private static NoContentResult result;

        Because of = () =>
            result = (NoContentResult)roleController.Delete(RoleId).Await();

        It should_return_no_content_code = () =>
            result.StatusCode.ShouldEqual(204);
    }

    internal class When_decorating_Role_Delete_method : RoleControllerSpecs
    {
        private static MethodInfo methodInfo;

        Because of = () =>
            methodInfo = typeof(RoleController).GetMethod("Delete");

        It should_have_CanDelteRoles_policy = () =>
            methodInfo.Should()
            .BeDecoratedWith<AuthorizeAttribute>(a => a.Policy == "CanDeleteRoles");
    }
}