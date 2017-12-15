using BellRichM.Attribute.CodeCoverage;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellRichM.Identity.Api.Data
{
    [ExcludeFromCodeCoverage]
    public class Role : IdentityRole
    {
        public string Description { get; set; }

        [NotMapped]
        public IEnumerable<ClaimValue> ClaimValues { get; set; }
    }
}
