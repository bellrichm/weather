using System.Threading.Tasks;
using BellRichM.Identity.Api.Data;

namespace BellRichM.Identity.Api.Repositories
{
    public interface IRoleRepository
    {
        Task<Role> GetById(string id);

        Task<Role> Create(Role role);

        Task Delete(string id);
    }
}