using System.Threading.Tasks;
using BellRichM.Identity.Api.Data;

namespace BellRichM.Identity.Api.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetById(string id);

        Task<User> GetByName(string name);

        Task<User> Create(User user, string password);

        Task Delete(string id);
    }
}