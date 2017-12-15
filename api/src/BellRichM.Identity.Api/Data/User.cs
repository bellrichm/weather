using BellRichM.Attribute.CodeCoverage;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace BellRichM.Identity.Api.Data
{
    [ExcludeFromCodeCoverage]
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        [NotMapped]
        public IEnumerable<Role> Roles { get; set; }
    }
}