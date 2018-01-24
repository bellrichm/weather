using BellRichM.Attribute.CodeCoverage;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellRichM.Identity.Api.Data
{
    /// <summary>
    /// The role.
    /// </summary>
    /// <seealso cref="Microsoft.AspNetCore.Identity.IdentityRole" />
    [ExcludeFromCodeCoverage]
    public class Role : IdentityRole
    {
        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the claim values.
        /// </summary>
        /// <value>
        /// The <see cref="IEnumerable{ClaimValue}"/>.
        /// </value>
        [NotMapped]
        public IEnumerable<ClaimValue> ClaimValues { get; set; }
    }
}
