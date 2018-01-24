using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace BellRichM.Identity.Api.Data
{
    /// <summary>
    /// The context of the identity database
    /// </summary>
    public interface IIdentityDbContext
    {
        /// <summary>
        /// Begins the transaction.
        /// </summary>
        /// <returns>The <see cref="IDbContextTransaction"/></returns>
        IDbContextTransaction BeginTransaction();
    }
}