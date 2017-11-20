using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;
using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Identity.Api.Data
{
    [ExcludeFromCodeCoverage]    
    public class User : IdentityUser
    {
        public string FirstName {get; set;}
        
        public string LastName {get; set;}
            
        [NotMapped]
        public IEnumerable<Role> Roles {get; set;}      
    }
}