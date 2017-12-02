using System.Threading.Tasks;

namespace BellRichM.Identity.Api.Services
{
    public interface IJwtManager
    {
         Task<string> GenerateToken(string userName, string passWord);
    }
}