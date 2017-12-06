using System.ComponentModel.DataAnnotations;
using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Identity.Api.Models
{
    [ExcludeFromCodeCoverage]
    public class UserLoginModel
    {
        [Required]
        public string UserName {get; set;}
        [Required]
        public string Password {get; set;}       
    }
}