using System.Collections.Generic;
using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Identity.Api.Models
{
    [ExcludeFromCodeCoverage]
    public class UserModel
    {
        public string UserName { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Id { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool TwoFactorEnabled { get; set; }

        public bool LockoutEnabled { get; set; }

        public IEnumerable<RoleModel> Roles { get; set; }
    }
}