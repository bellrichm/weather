using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

using BellRichM.Identity.Api.Configuration;
using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Repositories;

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace BellRichM.Identity.Api.Services
{
    public class JwtManager : IJwtManager
    {
        private readonly ILogger _logger;
        private readonly IJwtConfiguration _jwtConfiguration;
        private readonly IUserRepository _userRepository;
        private readonly SignInManager<User> _signInManager;

        public JwtManager(IJwtConfiguration jwtConfiguration, ILogger<JwtManager> logger, IUserRepository userRepository, SignInManager<User> signInManager)
        {
            _jwtConfiguration = jwtConfiguration;
            _logger = logger;
            _userRepository = userRepository;
            _signInManager = signInManager;
        }

        public async Task<string> GenerateToken(string userName, string passWord)
        {
            var user = await _userRepository.GetByName(userName);
            if (user == null)
            {
                return null;
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, passWord,  false);
            if (!result.Succeeded)
            {
                return null;
            }

            var now = DateTime.UtcNow;
            var claims = BuildClaims(user, now);
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_jwtConfiguration.SecretKey));
            var jwt = new JwtSecurityToken(
                issuer: _jwtConfiguration.Issuer,
                audience: _jwtConfiguration.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(_jwtConfiguration.ValidFor),
                signingCredentials: new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }

        private static long ToUnixEpochDate(DateTime date) => new DateTimeOffset(date).ToUniversalTime().ToUnixTimeSeconds();

        private List<Claim> BuildClaims(User user, DateTime now)
        {
            IdentityOptions options = new IdentityOptions();
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, ToUnixEpochDate(now).ToString(), ClaimValueTypes.Integer64),
                new Claim(options.ClaimsIdentity.UserIdClaimType, user.Id.ToString()),
                new Claim(options.ClaimsIdentity.UserNameClaimType, user.UserName)
            };

            // TODO: currently do not support claims on a user directly (create an issue for this)
            foreach (var role in user.Roles)
            {
                foreach (var claimValue in role.ClaimValues)
                {
                    claims.Add(new Claim(claimValue.Type, claimValue.Value, claimValue.ValueType));
                }
            }

            return claims;
        }
    }
}