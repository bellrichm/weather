using BellRichM.Attribute.CodeCoverage;
using BellRichM.Identity.Api.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BellRichM.Identity.Api.Data
{
    [ExcludeFromCodeCoverage]
    public class IdentityDbContext : IdentityDbContext<User, Role, string>, IIdentityDbContext
    {
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
            : base(options)
        {
        }

        public IDbContextTransaction BeginTransaction()
        {
             return Database.BeginTransaction();
        }
    }
}