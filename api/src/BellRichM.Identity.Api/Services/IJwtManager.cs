using System.Threading.Tasks;

namespace BellRichM.Identity.Api.Services
{
    /// <summary>
    /// Manages the Json Web Token (JWT).
    /// </summary>
    public interface IJwtManager
    {
        /// <summary>
        /// Generates the token.
        /// </summary>
        /// <param name="userName">Name of the user.</param>
        /// <param name="password">The password for the user.</param>
        /// <returns>The Json Web Token (JWT).</returns>
        Task<string> GenerateToken(string userName, string password);
    }
}