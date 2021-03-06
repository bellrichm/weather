using BellRichM.Helpers.Test;
using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Exceptions;
using BellRichM.Identity.Api.Repositories;
using BellRichM.Logging;
using FluentAssertions;
using Machine.Specifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;

using IT = Moq.It;
using It = Machine.Specifications.It;

namespace BellRichM.Identity.Api.Test
{
  internal class RoleRepositorySpecs
  {
    protected static LoggingData loggingData;

    protected static Mock<ILoggerAdapter<RoleRepository>> loggerMock;
    protected static Mock<RoleManager<Role>> roleManagerMock;
    protected static Mock<IdentityDbContext> identityDbContextMock;
    protected static Mock<IRoleStore<Role>> roleStoreMock;
    protected static Mock<IDbContextTransaction> dbTransactionMock;

    protected static RoleRepository roleRepository;
    protected static Role role;
    protected static List<Claim> claims;
    protected static Claim claim;

    protected static List<ClaimValue> claimValues;
    Establish context = () =>
    {
      loggerMock = new Mock<ILoggerAdapter<RoleRepository>>();
      roleStoreMock = new Mock<IRoleStore<Role>>();
      roleManagerMock = new Mock<RoleManager<Role>>(roleStoreMock.Object, null, null, null, null);
      dbTransactionMock = new Mock<IDbContextTransaction>();
      identityDbContextMock = new Mock<IdentityDbContext>(new DbContextOptions<IdentityDbContext>());
      identityDbContextMock.Setup(x => x.BeginTransaction()).Returns(dbTransactionMock.Object);

      role = new Role
      {
        Id = "id",
        Description = "description",
        Name = "name"
      };

      claimValues = new List<ClaimValue>();
      claimValues.Add(new ClaimValue { Type = "type", Value = "value" });

      claims = new List<Claim>();

      roleManagerMock.Setup(x => x.FindByIdAsync(role.Id))
        .ReturnsAsync(role);
      roleManagerMock.Setup(x => x.FindByNameAsync(role.Name))
        .ReturnsAsync(role);
      roleManagerMock.Setup(x => x.GetClaimsAsync(role))
        .ReturnsAsync(claims);

      roleRepository = new RoleRepository(loggerMock.Object, roleManagerMock.Object, identityDbContextMock.Object);
    };

    Cleanup after = () =>
      roleRepository.Dispose();
  }

  [Subject("Get Role")]
  internal class When_role_id_does_not_exist : RoleRepositorySpecs
  {
    private static Role roleResult;

    Establish context = () =>
    {
      loggingData = new LoggingData
      {
          DebugTimes = 1,
          EventLoggingData = new List<EventLoggingData>(),
          ErrorLoggingMessages = new List<string>()
      };

      roleManagerMock.Setup(x => x.FindByIdAsync(role.Id))
        .ReturnsAsync((Role)null);
    };

    Because of = () =>
      roleResult = roleRepository.GetById(role.Id).Result;

    Behaves_like<LoggingBehaviors<RoleRepository>> correct_logging = () => { };

    It should_return_correct_role = () =>
      roleResult.ShouldBeNull();
  }

  internal class When_getting_role_by_id_without_claims : RoleRepositorySpecs
  {
    private static Role roleResult;

    Establish context = () =>
    {
        loggingData = new LoggingData
        {
            DebugTimes = 1,
            EventLoggingData = new List<EventLoggingData>(),
            ErrorLoggingMessages = new List<string>()
        };
    };

    Because of = () =>
      roleResult = roleRepository.GetById(role.Id).Result;

    Behaves_like<LoggingBehaviors<RoleRepository>> correct_logging = () => { };

    It should_return_correct_role = () =>
      roleResult.Should().BeEquivalentTo(role);

    It should_have_no_claims = () =>
      roleResult.ClaimValues.Should().BeEmpty();
  }

  internal class When_getting_role_by_id_with_claims : RoleRepositorySpecs
  {
    private static Role roleResult;

    Establish context = () =>
    {
      claim = new Claim("type", "value", "description");
      claims.Add(claim);

      loggingData = new LoggingData
      {
          DebugTimes = 1,
          EventLoggingData = new List<EventLoggingData>(),
          ErrorLoggingMessages = new List<string>()
      };
    };

    Because of = () =>
      roleResult = roleRepository.GetById(role.Id).Result;

    Behaves_like<LoggingBehaviors<RoleRepository>> correct_logging = () => { };

    It should_return_correct_role = () =>
      roleResult.Should().BeEquivalentTo(role);

    It should_have_one_claim = () =>
      roleResult.ClaimValues.Should().ContainSingle();

    It should_have_correct_claim_values = () =>
    {
      roleResult.ClaimValues.Should()
        .AllBeEquivalentTo(claim, config => config
            .Excluding(ctx => ctx.OriginalIssuer)
            .Excluding(ctx => ctx.Properties)
            .Excluding(ctx => ctx.Subject)
            .Excluding(ctx => ctx.Issuer));
    };
  }

  internal class When_role_name_does_not_exist : RoleRepositorySpecs
  {
    private static Role roleResult;

    Establish context = () =>
    {
      roleManagerMock.Setup(x => x.FindByNameAsync(role.Name))
        .ReturnsAsync((Role)null);
      loggingData = new LoggingData
      {
          DebugTimes = 1,
          EventLoggingData = new List<EventLoggingData>(),
          ErrorLoggingMessages = new List<string>()
      };
    };

    Because of = () =>
      roleResult = roleRepository.GetByName(role.Id).Result;

    Behaves_like<LoggingBehaviors<RoleRepository>> correct_logging = () => { };

    It should_return_correct_role = () =>
      roleResult.ShouldBeNull();
  }

  internal class When_getting_role_by_name_without_claims : RoleRepositorySpecs
  {
    private static Role roleResult;

    Establish context = () =>
    {
        loggingData = new LoggingData
        {
            DebugTimes = 1,
            EventLoggingData = new List<EventLoggingData>(),
            ErrorLoggingMessages = new List<string>()
        };
    };

    Because of = () =>
      roleResult = roleRepository.GetByName(role.Name).Result;

    Behaves_like<LoggingBehaviors<RoleRepository>> correct_logging = () => { };

    It should_return_correct_role = () =>
      roleResult.Should().BeEquivalentTo(role);

    It should_have_no_claims = () =>
      roleResult.ClaimValues.Should().BeEmpty();
  }

  internal class When_getting_role_by_name_with_claims : RoleRepositorySpecs
  {
    private static Role roleResult;

    Establish context = () =>
    {
        claim = new Claim("type", "value", "description");
        claims.Add(claim);
        loggingData = new LoggingData
        {
            DebugTimes = 1,
            EventLoggingData = new List<EventLoggingData>(),
            ErrorLoggingMessages = new List<string>()
        };
    };

    Because of = () =>
      roleResult = roleRepository.GetByName(role.Name).Result;

    Behaves_like<LoggingBehaviors<RoleRepository>> correct_logging = () => { };

    It should_return_correct_role = () =>
      roleResult.Should().BeEquivalentTo(role);

    It should_have_one_claim = () =>
      roleResult.ClaimValues.Should().ContainSingle();

    It should_have_correct_claim_values = () =>
    {
      roleResult.ClaimValues.Should()
        .AllBeEquivalentTo(claim, config => config
            .Excluding(ctx => ctx.OriginalIssuer)
            .Excluding(ctx => ctx.Properties)
            .Excluding(ctx => ctx.Subject)
            .Excluding(ctx => ctx.Issuer));
    };
  }

  [Subject("creating Role")]
  internal class When_error_creating_role : RoleRepositorySpecs
  {
    private static Role roleResult;
    private static Exception exception;

    Establish context = () =>
    {
        roleManagerMock.Setup(x => x.CreateAsync(role))
          .ReturnsAsync(IdentityResult.Failed());

        loggingData = new LoggingData
        {
            DebugTimes = 1,
            EventLoggingData = new List<EventLoggingData>(),
            ErrorLoggingMessages = new List<string>()
        };
    };

    Because of = () =>
      exception = Catch.Exception(() => roleResult = roleRepository.Create(role).Await());

    Behaves_like<LoggingBehaviors<RoleRepository>> correct_logging = () => { };

    It should_throw_correct_exception_type = () =>
      exception.ShouldBeOfExactType<CreateRoleException>();

    It should_have_correct_exception_code = () =>
      ((CreateRoleException)exception).Code.ShouldEqual(CreateRoleExceptionCode.CreateRoleFailed);

    It should_not_return_a_role = () =>
      roleResult.ShouldBeNull();

    It should_rollback_the_work = () =>
      dbTransactionMock.Verify(x => x.Rollback(), Times.Once);

    It should_not_commit_the_work = () =>
      dbTransactionMock.Verify(x => x.Commit(), Times.Never);
  }

  internal class When_error_adding_claim : RoleRepositorySpecs
  {
    private static Role roleResult;
    private static Exception exception;

    Establish context = () =>
    {
        roleManagerMock.Setup(x => x.CreateAsync(role))
          .ReturnsAsync(IdentityResult.Success);

        roleManagerMock
          .Setup(x => x.AddClaimAsync(role, IT.IsAny<Claim>()))
          .ReturnsAsync(IdentityResult.Failed());

        role.ClaimValues = claimValues;

        loggingData = new LoggingData
        {
            DebugTimes = 1,
            EventLoggingData = new List<EventLoggingData>(),
            ErrorLoggingMessages = new List<string>()
        };
    };

    Because of = () =>
      exception = Catch.Exception(() => roleResult = roleRepository.Create(role).Await());

    Behaves_like<LoggingBehaviors<RoleRepository>> correct_logging = () => { };

    It should_throw_correct_exception_type = () =>
      exception.ShouldBeOfExactType<CreateRoleException>();

    It should_have_correct_exception_code = () =>
      ((CreateRoleException)exception).Code.ShouldEqual(CreateRoleExceptionCode.AddClaimFailed);

    It should_not_return_a_role = () =>
      roleResult.ShouldBeNull();

    It should_rollback_the_work = () =>
      dbTransactionMock.Verify(x => x.Rollback(), Times.Once);

    It should_not_commit_the_work = () =>
      dbTransactionMock.Verify(x => x.Commit(), Times.Never);
  }

  internal class When_creating_role_without_claims : RoleRepositorySpecs
  {
    private static Role roleResult;
    private static Exception exception;

    Establish context = () =>
    {
      roleManagerMock.Setup(x => x.CreateAsync(role))
        .ReturnsAsync(IdentityResult.Success);

      loggingData = new LoggingData
      {
          DebugTimes = 2,
          EventLoggingData = new List<EventLoggingData>(),
          ErrorLoggingMessages = new List<string>()
      };
    };

    Because of = () =>
      exception = Catch.Exception(() => roleResult = roleRepository.Create(role).Await());

    Behaves_like<LoggingBehaviors<RoleRepository>> correct_logging = () => { };

    It should_return_a_role = () =>
      roleResult.ShouldNotBeNull();

    It should_not_rollback_the_work = () =>
      dbTransactionMock.Verify(x => x.Rollback(), Times.Never);

    It should_commit_the_work = () =>
      dbTransactionMock.Verify(x => x.Commit(), Times.Once);
  }

  internal class When_creating_role_with_claims : RoleRepositorySpecs
  {
    private static Role roleResult;
    private static Exception exception;

    Establish context = () =>
    {
      roleManagerMock.Setup(x => x.CreateAsync(role))
        .ReturnsAsync(IdentityResult.Success);

      roleManagerMock
        .Setup(x => x.AddClaimAsync(role, IT.IsAny<Claim>()))
        .ReturnsAsync(IdentityResult.Success);

      role.ClaimValues = claimValues;

      loggingData = new LoggingData
      {
          DebugTimes = 2,
          EventLoggingData = new List<EventLoggingData>(),
          ErrorLoggingMessages = new List<string>()
      };
    };

    Because of = () =>
        exception = Catch.Exception(() => roleResult = roleRepository.Create(role).Await());

    Behaves_like<LoggingBehaviors<RoleRepository>> correct_logging = () => { };

    It should_return_a_role = () =>
      roleResult.ShouldNotBeNull();

    It should_add_the_claim = () =>
      roleManagerMock.Verify(
        x => x.AddClaimAsync(
                IT.IsAny<Role>(),
                IT.Is<Claim>(c => c.Type == "type" && c.Value == "value")),
        Times.Once);

    It should_not_rollback_the_work = () =>
        dbTransactionMock.Verify(x => x.Rollback(), Times.Never);

    It should_commit_the_work = () =>
      dbTransactionMock.Verify(x => x.Commit(), Times.Once);
  }

  [Subject("Delete Role")]
  internal class When_deleting_role_succeeds : RoleRepositorySpecs
  {
    private static Exception exception;

    Establish context = () =>
    {
      roleManagerMock.Setup(x => x.FindByIdAsync(role.Id))
        .ReturnsAsync(role);

      roleManagerMock.Setup(x => x.DeleteAsync(role))
        .ReturnsAsync(IdentityResult.Success);

      loggingData = new LoggingData
      {
          DebugTimes = 1,
          EventLoggingData = new List<EventLoggingData>(),
          ErrorLoggingMessages = new List<string>()
      };
    };

    Because of = () =>
        exception = Catch.Exception(() => roleRepository.Delete(role.Id).Await());

    Behaves_like<LoggingBehaviors<RoleRepository>> correct_logging = () => { };

    It should_not_throw_exception = () =>
      exception.ShouldBeNull();
  }

  internal class When_deleting_role_fails : RoleRepositorySpecs
  {
      private static Exception exception;

      Establish context = () =>
      {
          roleManagerMock.Setup(x => x.FindByIdAsync(role.Id))
            .ReturnsAsync(role);

          roleManagerMock.Setup(x => x.DeleteAsync(role))
            .ReturnsAsync(IdentityResult.Failed());

          loggingData = new LoggingData
          {
            DebugTimes = 1,
            EventLoggingData = new List<EventLoggingData>(),
            ErrorLoggingMessages = new List<string>()
          };
    };

      Because of = () =>
        exception = Catch.Exception(() => roleRepository.Delete(role.Id).Await());

      Behaves_like<LoggingBehaviors<RoleRepository>> correct_logging = () => { };

      It should_throw_correct_exception_type = () =>
        exception.ShouldBeOfExactType<DeleteRoleException>();

      It should_have_correct_exception_code = () =>
        ((DeleteRoleException)exception).Code.ShouldEqual(DeleteRoleExceptionCode.DeleteRoleFailed);
  }

  internal class When_deleting_nonexistant_role : RoleRepositorySpecs
  {
    private static Exception exception;

    Establish context = () =>
    {
      roleManagerMock.Setup(x => x.FindByIdAsync(role.Id))
        .ReturnsAsync((Role)null);

      loggingData = new LoggingData
      {
          DebugTimes = 1,
          EventLoggingData = new List<EventLoggingData>(),
          ErrorLoggingMessages = new List<string>()
      };
    };

    Because of = () =>
      exception = Catch.Exception(() => roleRepository.Delete(role.Id).Await());

    Behaves_like<LoggingBehaviors<RoleRepository>> correct_logging = () => { };

    It should_throw_correct_exception_type = () =>
      exception.ShouldBeOfExactType<DeleteRoleException>();

    It should_have_correct_exception_code = () =>
      ((DeleteRoleException)exception).Code.ShouldEqual(DeleteRoleExceptionCode.RoleNotFound);
  }
}