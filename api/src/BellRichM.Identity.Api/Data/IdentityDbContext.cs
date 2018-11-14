using BellRichM.Attribute.CodeCoverage;
using BellRichM.Identity.Api.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace BellRichM.Identity.Api.Data
{
    /// <summary>
    /// The context of the identity database.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class IdentityDbContext : IdentityDbContext<User, Role, string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="IdentityDbContext"/> class.
        /// </summary>
        /// <param name="options">The <see cref="DbContextOptions{IdentityDbContext}"/>.</param>
        public IdentityDbContext(DbContextOptions<IdentityDbContext> options)
            : base(options)
        {
        }

        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <returns>The <see cref="IDbContextTransaction"/>.</returns>
        public virtual IDbContextTransaction BeginTransaction()
        {
             return Database.BeginTransaction();
        }
    }
}