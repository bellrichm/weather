using BellRichM.Attribute.CodeCoverage;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellRichM.Identity.Api.Data
{
    /// <summary>
    /// The user.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Identity.IdentityUser" />
    [ExcludeFromCodeCoverage]
    public class User : IdentityUser
    {
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
        /// Gets or sets the roles.
        /// </summary>
        /// <value>
        /// The <see cref="IEnumerable{Role}"/>.
        /// </value>
        [NotMapped]
        public IEnumerable<Role> Roles { get; set; }
    }
}