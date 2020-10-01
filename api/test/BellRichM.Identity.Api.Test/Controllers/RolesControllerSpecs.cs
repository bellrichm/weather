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
    public class RolesControllerSpecs
    {
        protected static LoggingData loggingData;
        protected static Mock<ILoggerAdapter<RolesController>> loggerMock;
        protected static Mock<IMapper> mapperMock;
        protected static Mock<IRoleRepository> roleRepositoryMock;

        protected static RolesController rolesController;

        protected static List<Role> roles;
        protected static List<RoleModel> rolesModel;

        Establish context = () =>
        {
            loggerMock = new Mock<ILoggerAdapter<RolesController>>();
            mapperMock = new Mock<IMapper>();
            roleRepositoryMock = new Mock<IRoleRepository>();

            roles = new List<Role>
            {
                new Role
                {
                    Id = "id 01",
                    Name = "role 01"
                },
                new Role
                {
                    Id = "id 02",
                    Name = "role 02"
                }
            };

            rolesModel = new List<RoleModel>
            {
                new RoleModel
                {
                    Id = "id 01",
                    Name = "role 01"
                },
                new RoleModel
                {
                    Id = "id 02",
                    Name = "role 02"
                }
            };

            roleRepositoryMock.Setup(x => x.GetRoles()).ReturnsAsync(roles);
            mapperMock.Setup(x => x.Map<List<RoleModel>>(roles)).Returns(rolesModel);

            rolesController = new RolesController(loggerMock.Object, mapperMock.Object, roleRepositoryMock.Object);
        };
    }

    internal class When_roles_are_found : RolesControllerSpecs
    {
        private static ObjectResult result;

        Establish context = () =>
        {
            loggingData = new LoggingData
            {
                EventLoggingData = new List<EventLoggingData>
                {
                    new EventLoggingData(
                        EventId.RolesController_Get,
                        string.Empty)
                },
                ErrorLoggingMessages = new List<string>()
            };
        };

        Because of = () =>
            result = (ObjectResult)rolesController.Get().Await();

        Behaves_like<LoggingBehaviors<RolesController>> correct_logging = () => { };

        It should_return_success_status_code = () =>
            result.StatusCode.ShouldEqual(200);

        It should_return_a_rolemodel = () =>
            result.Value.ShouldNotBeNull();

        It should_return_an_object_of_type_RoleModel = () =>
            result.Value.Should().BeOfType<List<RoleModel>>();

        It should_return_the_rolemodel = () =>
        {
            var roles = (List<RoleModel>)result.Value;
            roles.Should().Equals(rolesModel);
        };
    }

    internal class When_decorating_GetRoles_method : RolesControllerSpecs
    {
        private static MethodInfo methodInfo;

        Because of = () =>
            methodInfo = typeof(RolesController).GetMethod("Get");

        It should_have_CanViewUsers_policy = () =>
            methodInfo.Should()
            .BeDecoratedWith<AuthorizeAttribute>(a => a.Policy == "CanViewUsers");
    }
}