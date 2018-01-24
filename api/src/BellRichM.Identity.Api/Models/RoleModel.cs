using System.Collections.Generic;
using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Identity.Api.Models
{
    /// <summary>
    /// The role view model.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class RoleModel
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

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
        /// The <see cref="IEnumerable{ClaimValueModel}"/>.
        /// </value>
        public IEnumerable<ClaimValueModel> ClaimValues { get; set; }
    }
}