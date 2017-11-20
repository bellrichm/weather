using FluentAssertions;
using Machine.Specifications;
using Moq;

using IT = Moq.It;
using It = Machine.Specifications.It;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Exceptions;
using BellRichM.Identity.Api.Repositories;

namespace BellRichM.Identity.Api.Test.Repositories
{
    [Subject("Get User")]
    internal class when_user_does_not_exist : UserRepositorySpecs
    {
        private static User userResult;

        Establish context = () =>
            userManagerMock.Setup(x => x.FindByIdAsync(user.Id))
                .ReturnsAsync((User)null);

        Because of = () =>
            userResult  = userRepository.GetById(user.Id).Result;

        It should_return_correct_role = () =>
            userResult.ShouldBeNull();        
    }

    internal class when_getting_user_without_roles : UserRepositorySpecs
    {
        private static User userResult;

        Because of = () =>
            userResult  = userRepository.GetById(user.Id).Result;

        It should_return_correct_user = () =>
            userResult.ShouldBeEquivalentTo(user);

        It should_have_no_roles = () =>
            userResult.Roles.Should().BeEmpty();
    }

    internal class when_getting_user_with_roles : UserRepositorySpecs
    {
        private static User userResult;

        Establish context = () =>
            roleNames.Add(roleName);

        Because of = () =>
            userResult  = userRepository.GetById(user.Id).Result;

        It should_return_correct_role = () =>
            userResult.ShouldBeEquivalentTo(user);

        It should_have_one_claim = () =>
            userResult.Roles.Should().ContainSingle();

        It should_have_correct_claim_values = () =>
            userResult.Roles.ShouldAllBeEquivalentTo(role);
    }

    [Subject("creating User")]
    internal class when_error_creating_user : UserRepositorySpecs
    {
        private static User userResult;
        private static Exception exception;

        Establish context = () =>
        {
            var identityResult = new IdentityResult();
            identityResult = IdentityResult.Failed();    
            userManagerMock.Setup(x => x.CreateAsync(user, "P@ssw0rd"))
                .ReturnsAsync(identityResult);
        };

        Because of = ()  =>
            exception = Catch.Exception(() => userResult = userRepository.Create(user, "P@ssw0rd").Await());
 
         It should_throw_correct_exception_type = () =>
            exception.ShouldBeOfExactType<CreateUserException>();	
 
         It should_have_correct_exception_code = () =>
            ((CreateUserException)exception).Code.ShouldEqual(CreateUserExceptionCode.CreateUserFailed);
        
        It should_not_return_a_user = () =>
            userResult.ShouldBeNull(); 
 
        It should_rollback_the_work = () =>
            dbTransactionProxyMock.Verify(x => x.Rollback(), Times.Once);

        It should_not_commit_the_work = () =>
            dbTransactionProxyMock.Verify(x => x.Commit(), Times.Never); 
    }

    internal class when_role_does_not_exist : UserRepositorySpecs
    {
        private static User userResult;
        private static Exception exception;
        
        Establish context = () =>
        {
            var identityResult = new IdentityResult();
            identityResult = IdentityResult.Success;    
            userManagerMock.Setup(x => x.CreateAsync(user, "P@ssw0rd"))
                .ReturnsAsync(identityResult);
            
            roles.Add(role);

            roleRepositoryMock.Setup(x => x.GetByName(roleName))
                .ReturnsAsync((Role)null);      
        };
        
        Because of = ()  =>
            exception = Catch.Exception(() => userResult = userRepository.Create(user, "P@ssw0rd").Await());
 
         It should_throw_correct_exception_type = () =>
            exception.ShouldBeOfExactType<CreateUserException>();	
 
         It should_have_correct_exception_code = () =>
            ((CreateUserException)exception).Code.ShouldEqual(CreateUserExceptionCode.RoleNotFound);
        
        It should_not_return_a_user = () =>
            userResult.ShouldBeNull(); 
 
        It should_rollback_the_work = () =>
            dbTransactionProxyMock.Verify(x => x.Rollback(), Times.Once);

        It should_not_commit_the_work = () =>
            dbTransactionProxyMock.Verify(x => x.Commit(), Times.Never);             
        
    }

    internal class when_error_adding_role : UserRepositorySpecs
    {
        private static User userResult;
        private static Exception exception;

        Establish context = () =>
        {
            var identityResult = new IdentityResult();
            identityResult = IdentityResult.Success;    
            userManagerMock.Setup(x => x.CreateAsync(user, "P@ssw0rd"))
                .ReturnsAsync(identityResult);
            
            var roleResult = new IdentityResult(); 
            roleResult = IdentityResult.Failed();
            userManagerMock
                .Setup(x => x.AddToRoleAsync(user, IT.IsAny<String>()))
                .ReturnsAsync(roleResult);
            
            roles.Add(role);
        };

        Because of = ()  =>
            exception = Catch.Exception(() => userResult = userRepository.Create(user, "P@ssw0rd").Await());
 
         It should_throw_correct_exception_type = () =>
            exception.ShouldBeOfExactType<CreateUserException>();	
 
         It should_have_correct_exception_code = () =>
            ((CreateUserException)exception).Code.ShouldEqual(CreateUserExceptionCode.AddRoleFailed);
        
        It should_not_return_a_user = () =>
            userResult.ShouldBeNull(); 
 
        It should_rollback_the_work = () =>
            dbTransactionProxyMock.Verify(x => x.Rollback(), Times.Once);

        It should_not_commit_the_work = () =>
            dbTransactionProxyMock.Verify(x => x.Commit(), Times.Never); 
    }

    internal class when_creating_user_without_roles : UserRepositorySpecs
    {
        private static User userResult;
        private static Exception exception;

        Establish context = () =>
        {
            var identityResult = new IdentityResult();
            identityResult = IdentityResult.Success;
            userManagerMock.Setup(x => x.CreateAsync(user, "P@ssw0rd"))
                .ReturnsAsync(identityResult);
        };

        Because of = ()  =>
            exception = Catch.Exception(() => userResult = userRepository.Create(user, "P@ssw0rd").Await());

        It should_return_a_user = () =>
            userResult.ShouldNotBeNull();
 
        It should_not_rollback_the_work = () =>
            dbTransactionProxyMock.Verify(x => x.Rollback(), Times.Never);

        It should_commit_the_work = () =>
            dbTransactionProxyMock.Verify(x => x.Commit(), Times.Once); 
    } 

    internal class when_creating_user_with_roles : UserRepositorySpecs
    {
        private static User userResult;
        private static Exception exception;
       
        Establish context = () =>
        {
            var identityResult = new IdentityResult();
            identityResult = IdentityResult.Success;
            userManagerMock.Setup(x => x.CreateAsync(user, "P@ssw0rd"))
                .ReturnsAsync(identityResult);

            var roleResult = new IdentityResult(); 
            roleResult = IdentityResult.Success;
            userManagerMock
                .Setup(x => x.AddToRoleAsync(user, roleName))
                .ReturnsAsync(roleResult);

            roles.Add(role);
        };

        Because of = ()  =>
            exception = Catch.Exception(() => userResult = userRepository.Create(user, "P@ssw0rd").Await());
  
        It should_return_a_user = () =>
            userResult.ShouldNotBeNull();
 
        It should_add_the_role = () =>
            userManagerMock.Verify(x => x.AddToRoleAsync(
                                            IT.Is<User>(u => u == user),
                                            IT.Is<string>(s => s == roleName)),
                                        Times.Once);

        It should_not_rollback_the_work = () =>
            dbTransactionProxyMock.Verify(x => x.Rollback(), Times.Never);

        It should_commit_the_work = () =>
            dbTransactionProxyMock.Verify(x => x.Commit(), Times.Once); 
    } 

    [Subject("Delete User")] 
    internal class when_deleting_user_succeeds  : UserRepositorySpecs
    {
        private static Exception exception;

        Establish context = () =>
        {
            userManagerMock.Setup(x => x.FindByIdAsync(user.Id))
                .ReturnsAsync(user);

            var identityResult = new IdentityResult();
            identityResult = IdentityResult.Success;
            userManagerMock.Setup(x => x.DeleteAsync(user))
                .ReturnsAsync(identityResult);
                
        };

        Because of = () =>
            exception = Catch.Exception(() => userRepository.Delete(user.Id).Await());

         It should_not_throw_exception = () =>
            exception.ShouldBeNull();
    }
    
    internal class when_deleting_user_fails  : UserRepositorySpecs
    {
        private static Exception exception;

        Establish context = () =>
        {
            userManagerMock.Setup(x => x.FindByIdAsync(user.Id))
                .ReturnsAsync(user);

            var identityResult = new IdentityResult();
            identityResult = IdentityResult.Failed();
            userManagerMock.Setup(x => x.DeleteAsync(user))
                .ReturnsAsync(identityResult);
                
        };

        Because of = () =>
            exception = Catch.Exception(() => userRepository.Delete(user.Id).Await());

         It should_throw_correct_exception_type = () =>
            exception.ShouldBeOfExactType<DeleteUserException>();	
 
         It should_have_correct_exception_code = () =>
            ((DeleteUserException)exception).Code.ShouldEqual(DeleteUserExceptionCode.DeleteUserFailed);
    }

    internal class when_deleting_nonexistant_user : UserRepositorySpecs
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


    internal class UserRepositorySpecs
    {
        protected static Mock<ILogger<UserRepository>> loggerMock;
        protected static Mock<IRoleRepository> roleRepositoryMock;
        protected static Mock<IUserStore<User>> userStoreMock;
        protected static Mock<UserManager<User>> userManagerMock;
        protected static Mock<IIdentityDbContext> identityDbContextMock;
        protected static Mock<IDbContextTransactionProxy> dbTransactionProxyMock;
        
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
            dbTransactionProxyMock = new Mock<IDbContextTransactionProxy>();
            identityDbContextMock = new Mock<IIdentityDbContext>();
            identityDbContextMock.Setup(x => x.BeginTransaction()).Returns(dbTransactionProxyMock.Object);   

            roleNames = new List<string>();

            roles = new List<Role>();
            user = new User
            {
                Id = "id",
                UserName = "userName",
                Roles = roles
            };

            role = new Role{Name = roleName};

            userManagerMock.Setup(x => x.FindByIdAsync(user.Id))
                .ReturnsAsync(user);
            userManagerMock.Setup(x => x.GetRolesAsync(user))
                .ReturnsAsync(roleNames);  
            roleRepositoryMock.Setup(x => x.GetByName(roleName))
                .ReturnsAsync(role);      

            userRepository = new UserRepository(loggerMock.Object, roleRepositoryMock.Object, userManagerMock.Object, identityDbContextMock.Object);                        
        };                
    }
}