using System.Threading.Tasks;
using BellRichM.Identity.Api.Data;

namespace BellRichM.Identity.Api.Repositories
{
    /// <summary>
    /// The Role Repository.
    /// </summary>
    public interface IRoleRepository
    {
        /// <summary>
        /// Gets the role by its <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The identifier of the user.</param>
        /// <returns>The <see cref="Task{Role}"/>.</returns>
        Task<Role> GetById(string id);

        /// <summary>
        /// Gets the role by its <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the user.</param>
        /// <returns>The <see cref="Task{Role}"/>.</returns>
        Task<Role> GetByName(string name);

        /// <summary>
        /// Creates the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The <see cref="Role"/>.</param>
        /// <returns>The <see cref="Task{Role}"/>.</returns>
        Task<Role> Create(Role role);

        /// <summary>
        /// Deletes the role with the <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task Delete(string id);
    }
}
