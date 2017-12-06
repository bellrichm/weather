using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Identity.Api.Models
{
	[ExcludeFromCodeCoverage]
    public class ClaimValueModel 
    {
        public string Type {get; set;}
        public string Value {get; set;}
        public string ValueType {get; set;}
    }
}