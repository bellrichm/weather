using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Identity.Api.Data
{
    [ExcludeFromCodeCoverage]
    public class ClaimValue
    {
        public string Type { get; set; }

        public string Value { get; set; }

        public string ValueType { get; set; }
    }
}