using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace BellRichM.Identity.Api.Data
{
    public class Role : IdentityRole
    {
        public string Description { get; set; }
    }
}
