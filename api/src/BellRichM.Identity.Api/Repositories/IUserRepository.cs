using System.Collections.Generic;
using System.Threading.Tasks;
using BellRichM.Identity.Api.Data;

namespace BellRichM.Identity.Api.Repositories
{
    /// <summary>
    /// The User Repository.
    /// </summary>
    public interface IUserRepository
    {
        /// <summary>
        /// Gets the user by its <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The identifier of the user.</param>
        /// <returns>The <see cref="Task{User}"/>.</returns>
        Task<User> GetById(string id);

        /// <summary>
        /// Gets the user by its <paramref name="name"/>.
        /// </summary>
        /// <param name="name">The name of the user.</param>
        /// <returns>The <see cref="Task{User}"/>.</returns>
        Task<User> GetByName(string name);

        /// <summary>
        /// Creates the specified <paramref name="user"/> with a <paramref name="password"/>.
        /// </summary>
        /// <param name="user">The <see cref="User"/>.</param>
        /// <param name="password">The password for the user.</param>
        /// <returns>The <see cref="Task{User}"/>.</returns>
        Task<User> Create(User user, string password);

        /// <summary>
        /// Deletes the user with the <paramref name="id"/>.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The <see cref="Task"/>.</returns>
        Task Delete(string id);

        /// <summary>
        /// Gets users.
        /// </summary>
        /// <returns>The <see cref="Task{User}"/>.</returns>
        Task<IEnumerable<User>> GetUsers();
    }
}