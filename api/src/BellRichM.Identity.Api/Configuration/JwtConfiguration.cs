using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace BellRichM.Identity.Api.Configuration
{
    public class JwtConfiguration : IJwtConfiguration
    {
        public string Issuer { get; set; }

        public string Audience { get; set; }

        public TimeSpan ValidFor { get; set; } = TimeSpan.FromMinutes(5);

        public string SecretKey { get; set; }

        public void Validate()
        {
            if (string.IsNullOrEmpty(Issuer))
            {
                throw new ArgumentNullException(nameof(Issuer));
            }

            if (string.IsNullOrEmpty(Audience))
            {
                throw new ArgumentNullException(nameof(Audience));
            }

            if (string.IsNullOrEmpty(SecretKey))
            {
                throw new ArgumentNullException(nameof(SecretKey));
            }
        }
    }
}