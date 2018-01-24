using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Identity.Api.Data
{
    /// <summary>
    /// The claim value.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ClaimValue
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the type of the value.
        /// </summary>
        /// <value>
        /// The type of the value.
        /// </value>
        public string ValueType { get; set; }
    }
}