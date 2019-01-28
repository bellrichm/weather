using System;

namespace BellRichM.Identity.Api.Configuration
{
    /// <summary>
    /// The configuration for managing the JWT.
    /// </summary>
    public interface IJwtConfiguration
    {
        /// <summary>
        /// Gets or sets the issuer.
        /// </summary>
        /// <value>
        /// The issuer.
        /// </value>
        string Issuer { get; set; }

        /// <summary>
        /// Gets or sets the audience.
        /// </summary>
        /// <value>
        /// The audience.
        /// </value>
        string Audience { get; set; }

        /// <summary>
        /// Gets or sets the valid for.
        /// </summary>
        /// <value>
        /// The valid for.
        /// </value>
        TimeSpan ValidFor { get; set; }

        /// <summary>
        /// Gets or sets the secret key.
        /// </summary>
        /// <value>
        /// The secret key.
        /// </value>
        string SecretKey { get; set; }
    }
}