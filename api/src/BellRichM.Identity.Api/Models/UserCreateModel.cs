using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Identity.Api.Models
{
    /// <summary>
    /// The model for creating a user
    /// </summary>
    /// <seealso cref="BellRichM.Identity.Api.Models.UserModel" />
    [ExcludeFromCodeCoverage]
    public class UserCreateModel : UserModel
    {
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