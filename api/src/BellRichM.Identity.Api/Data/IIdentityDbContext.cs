using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;

namespace BellRichM.Identity.Api.Data
{
    public interface IIdentityDbContext
    {
        IDbContextTransaction BeginTransaction();
    }
}