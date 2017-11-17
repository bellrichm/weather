using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BellRichM.Identity.Api.Data;

namespace BellRichM.Identity.Api.Data
{
    public class IdentityDbContext : IdentityDbContext<User, Role, string>, IIdentityDbContext
    {

        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
            : base(options)
        {
        }

        public IDbContextTransactionProxy BeginTransaction()
        {
             return new DbContextTransactionProxy(this);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
        }
    }
}