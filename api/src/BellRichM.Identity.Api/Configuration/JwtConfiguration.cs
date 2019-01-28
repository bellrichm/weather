using System;
using System.ComponentModel.DataAnnotations;
using System.Text;
using BellRichM.Attribute.CodeCoverage;
using Destructurama.Attributed;

namespace BellRichM.Identity.Api.Configuration
{
    /// <inheritdoc/>
    [ExcludeFromCodeCoverage]
    public class JwtConfiguration : IJwtConfiguration
    {
        /// <inheritdoc/>
        [Required]
        public string Issuer { get; set; }

        /// <inheritdoc/>
        [Required]
        public string Audience { get; set; }

        /// <inheritdoc/>
        public TimeSpan ValidFor { get; set; } = TimeSpan.FromMinutes(5);

        /// <inheritdoc/>
        [NotLogged]
        [Required]
        public string SecretKey { get; set; }
    }
}