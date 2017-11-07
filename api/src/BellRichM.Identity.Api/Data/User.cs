using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BellRichM.Identity.Api.Data
{
    public class User : IdentityUser
    {
        public string FirstName {get; set;}
        public string LastName {get; set;}
    }
}