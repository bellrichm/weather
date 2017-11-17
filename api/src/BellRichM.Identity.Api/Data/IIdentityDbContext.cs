using Microsoft.EntityFrameworkCore.Infrastructure;

namespace BellRichM.Identity.Api.Data
{
    public interface IIdentityDbContext
    {
        DatabaseFacade Database {get;}
    }
}