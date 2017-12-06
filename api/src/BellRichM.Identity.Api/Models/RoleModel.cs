using System.Collections.Generic;
using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Identity.Api.Models
{
	[ExcludeFromCodeCoverage]
    public class RoleModel
    {
        public string Id {get; set;}
        public string Name {get; set;}
        public string Description {get; set;}
        public IEnumerable<ClaimValueModel> ClaimValues {get; set;}
    }
}