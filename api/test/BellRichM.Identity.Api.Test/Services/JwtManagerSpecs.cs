using FluentAssertions;
using Machine.Specifications;
using Moq;

using IT = Moq.It;
using It = Machine.Specifications.It;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using BellRichM.Identity.Api.Configuration;
using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Services;
using BellRichM.Identity.Api.Repositories;

namespace BellRichM.Identity.Api.Test.Services
{
    internal class when_user_does_not_exist : JwtManagerSpecs
    {
        Establish context = () => 
            userRepositoryMock.Setup(x => x.GetByName(user.UserName))
                .ReturnsAsync((User)null); 

        Because of = ()  =>
            jwt = jwtManager.GenerateToken(user.UserName, password).Await();

        It should_return_null_token = () =>
            jwt.ShouldBeNull();        
    };

    internal class when_login_fails : JwtManagerSpecs
    {
        Establish context = () =>
        {            
            signInResult = SignInResult.Failed;    
            signInManagerMock.Setup(x => x.CheckPasswordSignInAsync(user, password, false))
                .ReturnsAsync(signInResult);        
        };

        Because of = ()  =>
            jwt = jwtManager.GenerateToken(user.UserName, password).Await();        

        It should_return_null_token = () =>
            jwt.ShouldBeNull();        
    }

    internal class when_token_created : JwtManagerSpecs
    {
        private static JwtSecurityToken jwtSecurityToken;

        Because of = ()  =>
        {
            jwt = jwtManager.GenerateToken(user.UserName, password).Await();

            var principal = new JwtSecurityTokenHandler ()
                .ValidateToken (jwt, tokenValidationParameters, out var securityToken);
            jwtSecurityToken = (JwtSecurityToken) securityToken;
        };

        It should_have_correct_number_of_claims = () =>
            jwtSecurityToken.Claims.Count().ShouldBeEquivalentTo(10);
        
        It should_contain_sub_claim = () =>
            jwtSecurityToken.Claims.Should().
                ContainSingle(c => c.Type == JwtRegisteredClaimNames.Sub && c.Value == user.UserName);

        It should_contain_iss_claim = () =>
            jwtSecurityToken.Claims.Should().
                ContainSingle(c => c.Type == JwtRegisteredClaimNames.Iss && c.Value == issuer);

        It should_contain_aud_claim = () =>
            jwtSecurityToken.Claims.Should().
                ContainSingle(c => c.Type == JwtRegisteredClaimNames.Aud && c.Value == audience);

        It should_contain_nbf_claim = () =>
            jwtSecurityToken.Claims.Should().
                ContainSingle(c => c.Type == JwtRegisteredClaimNames.Nbf && !string.IsNullOrEmpty(c.Value));

        It should_contain_exp_claim = () =>
            jwtSecurityToken.Claims.Should().
                ContainSingle(c => c.Type == JwtRegisteredClaimNames.Exp && !string.IsNullOrEmpty(c.Value));

        It should_contain_jti_claim = () =>
            jwtSecurityToken.Claims.Should().
                ContainSingle(c => c.Type == JwtRegisteredClaimNames.Jti && !string.IsNullOrEmpty(c.Value));

        It should_contain_iat_claim = () =>
            jwtSecurityToken.Claims.Should().
                ContainSingle(c => c.Type == JwtRegisteredClaimNames.Iat && !string.IsNullOrEmpty(c.Value));
        It should_contain_userid_claim = () =>
            jwtSecurityToken.Claims.Should().
                ContainSingle(c => c.Type == options.ClaimsIdentity.UserIdClaimType && c.Value == user.Id);

        It should_contain_username_claim = () =>
            jwtSecurityToken.Claims.Should().
                ContainSingle(c => c.Type == options.ClaimsIdentity.UserNameClaimType && c.Value == user.UserName);

        It should_contain_role_claim = () =>
            jwtSecurityToken.Claims.Should().
                ContainSingle(c => c.Type == claimValue.Type && c.Value == claimValue.Value);
    }

    internal class JwtManagerSpecs
    {
        protected const string issuer = "issuer";
        protected const string audience = "audience";
        protected const string password = "P@ssw0rd";
        protected static Mock<IJwtConfiguration> jwtConfigurationMock;
        protected static Mock<ILogger<JwtManager>> loggerMock;
        protected static Mock<IUserRepository> userRepositoryMock;
        protected static Mock<SignInManager<User>> signInManagerMock;
        protected static Mock<UserManager<User>> userManagerMock;
        protected static Mock<IUserStore<User>> userStoreMock;
        protected static SymmetricSecurityKey signingKey;        
        protected static IdentityOptions options;
        protected static User user;
        protected static List<ClaimValue> claimValues;
        protected static List<Role> roles;
        protected static Role role;
        protected static ClaimValue claimValue;
        protected static string jwt;
        protected static SignInResult signInResult;
        protected static JwtManager jwtManager;
        protected static TokenValidationParameters tokenValidationParameters;
        Establish context = () =>
        {
            const string secretKey = "superdupersecretkey";

            jwtConfigurationMock = new Mock<IJwtConfiguration>();
            loggerMock = new Mock<ILogger<JwtManager>>();
            userRepositoryMock = new Mock<IUserRepository>();
            userStoreMock = new Mock<IUserStore<User>>();
            userManagerMock = new Mock<UserManager<User>>(userStoreMock.Object, null, null, null, null, null, null, null, null);
            signInManagerMock = new Mock<SignInManager<User>>(userManagerMock.Object, new Mock<IHttpContextAccessor>().Object, new Mock<IUserClaimsPrincipalFactory<User>>().Object, null, null, null);

            signingKey = new SymmetricSecurityKey (Encoding.ASCII.GetBytes (secretKey));

            options = new IdentityOptions ();

            roles = new List<Role>();
            user = new User
            {
                Id = "id",
                UserName = "userName",
                Roles = roles
            };

            claimValues = new List<ClaimValue>();
            claimValue = new ClaimValue{Type="type", Value="value"};
            claimValues.Add(claimValue);

           role = new Role();
           role.ClaimValues = claimValues;
           roles.Add(role);
                        
            tokenValidationParameters = new TokenValidationParameters {
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidAudience = audience
            };

            jwtConfigurationMock.SetupGet(x =>x.Audience).Returns(audience);
            jwtConfigurationMock.SetupGet(x =>x.Issuer).Returns(issuer);            
            jwtConfigurationMock.SetupGet(x =>x.ValidFor).Returns(TimeSpan.FromMinutes(5));            
            jwtConfigurationMock.SetupGet(x =>x.SecretKey).Returns(secretKey);

            userRepositoryMock.Setup(x => x.GetByName(user.UserName))
                .ReturnsAsync(user); 

            signInResult = new SignInResult();
            signInResult = SignInResult.Success;    
            signInManagerMock.Setup(x => x.CheckPasswordSignInAsync(user, password, false))
                .ReturnsAsync(signInResult);                

            jwtManager = new JwtManager(jwtConfigurationMock.Object, loggerMock.Object, userRepositoryMock.Object, signInManagerMock.Object);                            
        };
    }
}