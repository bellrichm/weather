using System;
using System.Text;
using Destructurama.Attributed;

namespace BellRichM.Identity.Api.Configuration
{
    /// <inheritdoc/>
    public class JwtConfiguration : IJwtConfiguration
    {
        /// <inheritdoc/>
        public string Issuer { get; set; }

        /// <inheritdoc/>
        public string Audience { get; set; }

        /// <inheritdoc/>
        public TimeSpan ValidFor { get; set; } = TimeSpan.FromMinutes(5);

        /// <inheritdoc/>
        [NotLogged]
        public string SecretKey { get; set; }

        /// <inheritdoc/>
        #pragma warning disable CA2208
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
        #pragma warning restore CA2208
    }
}