using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Exceptions;
using BellRichM.Identity.Api.Repositories;
using FluentAssertions;
using Machine.Specifications;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;

using IT = Moq.It;
using It = Machine.Specifications.It;

namespace BellRichM.Identity.Api.Test.Repositories
{
  internal class UserRepositorySpecs
  {
    protected static Mock<ILogger<UserRepository>> loggerMock;
    protected static Mock<IRoleRepository> roleRepositoryMock;
    protected static Mock<IUserStore<User>> userStoreMock;
    protected static Mock<UserManager<User>> userManagerMock;
    protected static Mock<IIdentityDbContext> identityDbContextMock;
    protected static Mock<IDbContextTransaction> dbTransactionMock;

    protected static UserRepository userRepository;
    protected static User user;
    protected static List<string> roleNames;
    protected static string roleName = "roleName01";
    protected static List<Role> roles;

    protected static Role role;

    Establish context = () =>
    {
      loggerMock = new Mock<ILogger<UserRepository>>();
      roleRepositoryMock = new Mock<IRoleRepository>();
      userStoreMock = new Mock<IUserStore<User>>();
      userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
      dbTransactionMock = new Mock<IDbContextTransaction>();
      identityDbContextMock = new Mock<IIdentityDbContext>();
      identityDbContextMock.Setup(x => x.BeginTransaction()).Returns(dbTransactionMock.Object);

      roleNames = new List<string>();

      roles = new List<Role>();
      user = new User
      {
        Id = "id",
        UserName = "userName",
        Roles = roles
      };

      role = new Role { Name = roleName };

      userManagerMock.Setup(x => x.FindByIdAsync(user.Id))
        .ReturnsAsync(user);
      userManagerMock.Setup(x => x.FindByNameAsync(user.UserName))
        .ReturnsAsync(user);
      userManagerMock.Setup(x => x.GetRolesAsync(user))
        .ReturnsAsync(roleNames);
      roleRepositoryMock.Setup(x => x.GetByName(roleName))
        .ReturnsAsync(role);

      userRepository = new UserRepository(loggerMock.Object, roleRepositoryMock.Object, userManagerMock.Object, identityDbContextMock.Object);
    };

    Cleanup after = () =>
      userRepository.Dispose();
  }

  [Subject("Get User")]
  internal class When_getting_user_by_name_and_does_not_exist : UserRepositorySpecs
  {
    private static User userResult;

    Establish context = () =>
      userManagerMock.Setup(x => x.FindByNameAsync(user.UserName))
        .ReturnsAsync((User)null);

    Because of = () =>
      userResult = userRepository.GetByName(user.UserName).Result;

    It should_return_null_user = () =>
      userResult.ShouldBeNull();
  }

  internal class When_getting_user_by_name_without_roles : UserRepositorySpecs
  {
    private static User userResult;

    Because of = () =>
      userResult = userRepository.GetByName(user.UserName).Result;

    It should_return_correct_user = () =>
      userResult.ShouldBeEquivalentTo(user);

    It should_have_no_roles = () =>
      userResult.Roles.Should().BeEmpty();
  }

  internal class When_getting_user_by_name_with_roles : UserRepositorySpecs
  {
    private static User userResult;

    Establish context = () =>
      roleNames.Add(roleName);

    Because of = () =>
      userResult = userRepository.GetByName(user.UserName).Result;

    It should_return_correct_role = () =>
      userResult.ShouldBeEquivalentTo(user);

    It should_have_one_claim = () =>
      userResult.Roles.Should().ContainSingle();

    It should_have_correct_claim_values = () =>
      userResult.Roles.ShouldAllBeEquivalentTo(role);
  }

  internal class When_getting_user_by_id_and_does_not_exist : UserRepositorySpecs
  {
    private static User userResult;

    Establish context = () =>
      userManagerMock.Setup(x => x.FindByIdAsync(user.Id))
        .ReturnsAsync((User)null);

    Because of = () =>
      userResult = userRepository.GetById(user.Id).Result;

    It should_return_null_user = () =>
      userResult.ShouldBeNull();
  }

  internal class When_getting_user_by_id_without_roles : UserRepositorySpecs
  {
    private static User userResult;

    Because of = () =>
      userResult = userRepository.GetById(user.Id).Result;

    It should_return_correct_user = () =>
      userResult.ShouldBeEquivalentTo(user);

    It should_have_no_roles = () =>
      userResult.Roles.Should().BeEmpty();
  }

  internal class When_getting_user_by_id_with_roles : UserRepositorySpecs
  {
    private static User userResult;

    Establish context = () =>
      roleNames.Add(roleName);

    Because of = () =>
      userResult = userRepository.GetById(user.Id).Result;

    It should_return_correct_role = () =>
      userResult.ShouldBeEquivalentTo(user);

    It should_have_one_claim = () =>
      userResult.Roles.Should().ContainSingle();

    It should_have_correct_claim_values = () =>
      userResult.Roles.ShouldAllBeEquivalentTo(role);
  }

  [Subject("creating User")]
  internal class When_error_creating_user : UserRepositorySpecs
  {
    private static User userResult;
    private static Exception exception;

    Establish context = () =>
    {
      userManagerMock.Setup(x => x.CreateAsync(user, "P@ssw0rd"))
        .ReturnsAsync(IdentityResult.Failed());
    };

    Because of = () =>
      exception = Catch.Exception(() => userResult = userRepository.Create(user, "P@ssw0rd").Await());

    It should_throw_correct_exception_type = () =>
      exception.ShouldBeOfExactType<CreateUserException>();

    It should_have_correct_exception_code = () =>
      ((CreateUserException)exception).Code.ShouldEqual(CreateUserExceptionCode.CreateUserFailed);

    It should_not_return_a_user = () =>
      userResult.ShouldBeNull();

    It should_rollback_the_work = () =>
      dbTransactionMock.Verify(x => x.Rollback(), Times.Once);

    It should_not_commit_the_work = () =>
      dbTransactionMock.Verify(x => x.Commit(), Times.Never);
  }

  internal class When_role_does_not_exist : UserRepositorySpecs
  {
    private static User userResult;
    private static Exception exception;

    Establish context = () =>
    {
      userManagerMock.Setup(x => x.CreateAsync(user, "P@ssw0rd"))
        .ReturnsAsync(IdentityResult.Success);

      roles.Add(role);

      roleRepositoryMock.Setup(x => x.GetByName(roleName))
        .ReturnsAsync((Role)null);
    };

    Because of = () =>
      exception = Catch.Exception(() => userResult = userRepository.Create(user, "P@ssw0rd").Await());

    It should_throw_correct_exception_type = () =>
      exception.ShouldBeOfExactType<CreateUserException>();

    It should_have_correct_exception_code = () =>
      ((CreateUserException)exception).Code.ShouldEqual(CreateUserExceptionCode.RoleNotFound);

    It should_not_return_a_user = () =>
      userResult.ShouldBeNull();

    It should_rollback_the_work = () =>
      dbTransactionMock.Verify(x => x.Rollback(), Times.Once);

    It should_not_commit_the_work = () =>
      dbTransactionMock.Verify(x => x.Commit(), Times.Never);
  }

  internal class When_error_adding_role : UserRepositorySpecs
  {
    private static User userResult;
    private static Exception exception;

    Establish context = () =>
    {
      userManagerMock.Setup(x => x.CreateAsync(user, "P@ssw0rd"))
        .ReturnsAsync(IdentityResult.Success);

      userManagerMock
        .Setup(x => x.AddToRoleAsync(user, IT.IsAny<string>()))
        .ReturnsAsync(IdentityResult.Failed());

      roles.Add(role);
    };

    Because of = () =>
      exception = Catch.Exception(() => userResult = userRepository.Create(user, "P@ssw0rd").Await());

    It should_throw_correct_exception_type = () =>
      exception.ShouldBeOfExactType<CreateUserException>();

    It should_have_correct_exception_code = () =>
      ((CreateUserException)exception).Code.ShouldEqual(CreateUserExceptionCode.AddRoleFailed);

    It should_not_return_a_user = () =>
      userResult.ShouldBeNull();

    It should_rollback_the_work = () =>
      dbTransactionMock.Verify(x => x.Rollback(), Times.Once);

    It should_not_commit_the_work = () =>
      dbTransactionMock.Verify(x => x.Commit(), Times.Never);
  }

  internal class When_creating_user_without_roles : UserRepositorySpecs
  {
    private static User userResult;
    private static Exception exception;

    Establish context = () =>
    {
      userManagerMock.Setup(x => x.CreateAsync(user, "P@ssw0rd"))
        .ReturnsAsync(IdentityResult.Success);
    };

    Because of = () =>
      exception = Catch.Exception(() => userResult = userRepository.Create(user, "P@ssw0rd").Await());

    It should_return_a_user = () =>
      userResult.ShouldNotBeNull();

    It should_not_rollback_the_work = () =>
      dbTransactionMock.Verify(x => x.Rollback(), Times.Never);

    It should_commit_the_work = () =>
      dbTransactionMock.Verify(x => x.Commit(), Times.Once);
  }

  internal class When_creating_user_with_roles : UserRepositorySpecs
  {
    private static User userResult;
    private static Exception exception;

    Establish context = () =>
    {
      userManagerMock.Setup(x => x.CreateAsync(user, "P@ssw0rd"))
        .ReturnsAsync(IdentityResult.Success);

      userManagerMock
        .Setup(x => x.AddToRoleAsync(user, roleName))
        .ReturnsAsync(IdentityResult.Success);

      roles.Add(role);
    };

    Because of = () =>
      exception = Catch.Exception(() => userResult = userRepository.Create(user, "P@ssw0rd").Await());

    It should_return_a_user = () =>
      userResult.ShouldNotBeNull();

    It should_add_the_role = () =>
      userManagerMock.Verify(
              x => x.AddToRoleAsync(
                IT.Is<User>(u => u == user),
                IT.Is<string>(s => s == roleName)),
              Times.Once);

    It should_not_rollback_the_work = () =>
      dbTransactionMock.Verify(x => x.Rollback(), Times.Never);

    It should_commit_the_work = () =>
      dbTransactionMock.Verify(x => x.Commit(), Times.Once);
  }

  [Subject("Delete User")]
  internal class When_deleting_user_succeeds : UserRepositorySpecs
  {
    private static Exception exception;

    Establish context = () =>
    {
      userManagerMock.Setup(x => x.FindByIdAsync(user.Id))
        .ReturnsAsync(user);

      userManagerMock.Setup(x => x.DeleteAsync(user))
        .ReturnsAsync(IdentityResult.Success);
    };

    Because of = () =>
      exception = Catch.Exception(() => userRepository.Delete(user.Id).Await());

    It should_not_throw_exception = () =>
      exception.ShouldBeNull();
  }

  internal class When_deleting_user_fails : UserRepositorySpecs
  {
    private static Exception exception;

    Establish context = () =>
    {
      userManagerMock.Setup(x => x.FindByIdAsync(user.Id))
        .ReturnsAsync(user);

      userManagerMock.Setup(x => x.DeleteAsync(user))
        .ReturnsAsync(IdentityResult.Failed());
    };

    Because of = () =>
      exception = Catch.Exception(() => userRepository.Delete(user.Id).Await());

    It should_throw_correct_exception_type = () =>
      exception.ShouldBeOfExactType<DeleteUserException>();

    It should_have_correct_exception_code = () =>
      ((DeleteUserException)exception).Code.ShouldEqual(DeleteUserExceptionCode.DeleteUserFailed);
  }

  internal class When_deleting_nonexistant_user : UserRepositorySpecs
  {
    private static Exception exception;

    Establish context = () =>
      userManagerMock.Setup(x => x.FindByIdAsync(user.Id))
        .ReturnsAsync((User)null);

    Because of = () =>
      exception = Catch.Exception(() => userRepository.Delete(user.Id).Await());

    It should_throw_correct_exception_type = () =>
      exception.ShouldBeOfExactType<DeleteUserException>();

    It should_have_correct_exception_code = () =>
      ((DeleteUserException)exception).Code.ShouldEqual(DeleteUserExceptionCode.UserNotFound);
  }
}