using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Identity.Api.Models
{
    /// <summary>
    /// The User View Model
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class UserModel
    {
        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>
        /// The name of the user.
        /// </value>
        [Required]
        [RegularExpression(@"^[a-zA-Z0-9]*$", ErrorMessage="UserName must be alphanumeric")]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        [EmailAddress]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>
        /// The phone number.
        /// </value>
        [Phone]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether two factor login is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if two factor login is enabled; otherwise, <c>false</c>.
        /// </value>
        public bool TwoFactorEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether lockout is enabled for invalid logins.
        /// </summary>
        /// <value>
        ///   <c>true</c> if lockout is enabled for invalid logins; otherwise, <c>false</c>.
        /// </value>
        public bool LockoutEnabled { get; set; }

        /// <summary>
        /// Gets or sets the roles.
        /// </summary>
        /// <value>
        /// The <see cref="IEnumerable{RoleModel}"/>.
        /// </value>
        public IEnumerable<RoleModel> Roles { get; set; }
    }
}