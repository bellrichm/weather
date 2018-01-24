using System.ComponentModel.DataAnnotations;
using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Identity.Api.Models
{
    /// <summary>
    /// The model for logging a user in.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class UserLoginModel
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        [Required]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [Required]
        public string Password { get; set; }
    }
}