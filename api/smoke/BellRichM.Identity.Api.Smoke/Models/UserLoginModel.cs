using System.ComponentModel.DataAnnotations;

namespace BellRichM.Identity.Api.Smoke.Models
{
    public class UserLoginModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }
    }
}