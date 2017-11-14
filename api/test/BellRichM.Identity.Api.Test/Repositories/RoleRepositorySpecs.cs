using FluentAssertions;
using Machine.Specifications;
using Moq;

using It = Machine.Specifications.It;

using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Repositories;

namespace BellRichM.Identity.Api.Test 
{
    [Subject("Get Role")]
    internal class when_role_does_not_exist : RoleRepositorySpecs
    {
        Establish context = () =>
        {
            roleManagerMock.Setup(x => x.FindByIdAsync(role.Id))
                .ReturnsAsync((Role)null);
        };

        Because of = () =>
        {
            result  = roleReposity.GetById(role.Id);
            roleResult = result.Result;
        };

        It should_return_correct_role = () =>
        {
            roleResult.ShouldBeNull();
        };
    }
    
    internal class when_getting_role_without_claims : RoleRepositorySpecs
    {
        Because of = () =>
        {
            result  = roleReposity.GetById(role.Id);
            roleResult = result.Result;
        };

        It should_return_correct_role = () =>
        {
            roleResult.ShouldBeEquivalentTo(role);
        };

        It should_have_no_claims = () =>
        {
            roleResult.ClaimValues.Should().BeEmpty();
        };
    }

    internal class when_getting_role_with_claims : RoleRepositorySpecs
    {
        protected static Claim claim;

        Establish context = () =>
        {
            claim = new Claim("type", "value", "description");
            Claims.Add(claim);
        };

        Because of = () =>
        {
            result  = roleReposity.GetById(role.Id);
            roleResult = result.Result;
        };

        It should_return_correct_role = () =>
        {
            roleResult.ShouldBeEquivalentTo(role);
        };

        It should_have_one_claim = () =>
        {
            roleResult.ClaimValues.Should().ContainSingle();

        };

        It should_have_correct_claim_values = () =>
        {
            roleResult.ClaimValues.ShouldAllBeEquivalentTo(claim);
        };
    }

    internal class  RoleRepositorySpecs 
    {
        protected static Mock<ILogger<RoleRepository>> loggerMock;
        protected static Mock<RoleManager<Role>> roleManagerMock;
        protected static Mock<IRoleStore<Role>> roleStoreMock;
                                            
        protected static Task<Role> result;
        protected static RoleRepository roleReposity;
        protected static Role role;
        protected static List<Claim> Claims;
        protected static Role roleResult;
        Establish context = () =>
        {
            loggerMock = new Mock<ILogger<RoleRepository>>();
            roleStoreMock = new Mock<IRoleStore<Role>>();
            roleManagerMock = new Mock<RoleManager<Role>>(roleStoreMock.Object, null, null, null, null);
                
            role = new Role
            {
                Id = "id",
                Description = "description",
                Name = "name"
            };

            Claims = new List<Claim>();

            roleManagerMock.Setup(x => x.FindByIdAsync(role.Id))
                .ReturnsAsync(role);
            roleManagerMock.Setup(x => x.GetClaimsAsync(role))
                .ReturnsAsync(Claims);
            
            roleReposity = new RoleRepository(loggerMock.Object, roleManagerMock.Object);            
        };
    }   
}