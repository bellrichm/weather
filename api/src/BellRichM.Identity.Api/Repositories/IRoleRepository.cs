using System.Collections.Generic;
using System.Threading.Tasks;
using BellRichM.Identity.Api.Data;

namespace BellRichM.Identity.Api.Repositories
{
    public interface IRoleRepository
    {
        Task<Role> GetById(string id);

        Task<Role> GetByName(string name);

        Task<Role> Create(Role role);

        Task Delete(string id);
    }
}