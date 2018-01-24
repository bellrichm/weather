using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Identity.Api.Models
{
    /// <summary>
    /// The access token view model.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class AccessTokenModel
    {
        /// <summary>
        /// Gets or sets the json web token.
        /// </summary>
        /// <value>
        /// The json web token.
        /// </value>
        public string JsonWebToken { get; set; }
    }
}