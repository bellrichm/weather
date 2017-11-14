using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Identity.Api.Data
{
    [ExcludeFromCodeCoverage]    
    public class Role : IdentityRole
    {
        public string Description { get; set; }
        
        [NotMapped]
        public IEnumerable<ClaimValue> ClaimValues {get; set;}      
    }
}
