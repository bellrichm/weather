using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace BellRichM.Identity.Api.Services {
    public class JwtManager : IJwtManager {
        // TODO: Move to config file
        private const string issuer = "issuer";
        private const string audience = "audience";
        private const double validForMinutes = 5;
        private TimeSpan validFor;
        
        // TODO: Manage this outside of code
        private const string secretKey = "superdupersecretkey";

        private SymmetricSecurityKey _signingKey;
        private readonly ILogger _logger;
        private readonly IUserRepository _userRepository;
        private readonly SignInManager<User> _signInManager;

        public JwtManager (ILogger<JwtManager> logger, IUserRepository userRepository, SignInManager<User> signInManager) {
            _logger = logger;
            _userRepository = userRepository;
            _signInManager = signInManager;
            _signingKey = new SymmetricSecurityKey (Encoding.ASCII.GetBytes (secretKey));
        }

        public async Task<string> GenerateToken (string userId, string passWord) {
            var user = await _userRepository.GetById (userId);
            if (user == null)
            {
                return null;
            }

            var result = await _signInManager.PasswordSignInAsync (user, passWord, true, false);
            if (!result.Succeeded)
            {
                return null;
            }

            var now = DateTime.UtcNow;
            var claims = BuildClaims (user, now);
            validFor = TimeSpan.FromMinutes(Convert.ToDouble(validForMinutes));
            var jwt = new JwtSecurityToken (
                issuer: issuer,
                audience: audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(validFor),
                signingCredentials : new SigningCredentials (_signingKey, SecurityAlgorithms.HmacSha256)
            );

            var encodedJwt = new JwtSecurityTokenHandler ().WriteToken (jwt);
            return encodedJwt;
        }

        private List<Claim> BuildClaims (User user, DateTime now) {
            IdentityOptions _options = new IdentityOptions ();
            var claims = new List<Claim> {
                new Claim (JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim (JwtRegisteredClaimNames.Jti, Guid.NewGuid ().ToString ()),
                new Claim (JwtRegisteredClaimNames.Iat, ToUnixEpochDate (DateTime.Now).ToString (), ClaimValueTypes.Integer64),
                new Claim (_options.ClaimsIdentity.UserIdClaimType, user.Id.ToString ()),
                new Claim (_options.ClaimsIdentity.UserNameClaimType, user.UserName)
            };

            // TODO: currently do not support claims on a user directly (create an issue for this)
            foreach (var role in user.Roles) {
                foreach (var claimValue in role.ClaimValues) {
                    claims.Add (new Claim (claimValue.Type, claimValue.Value, claimValue.ValueType));
                }
            }

            return claims;
        }

        private static long ToUnixEpochDate (DateTime date) => new DateTimeOffset (date).ToUniversalTime ().ToUnixTimeSeconds ();
    }
}