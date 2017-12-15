using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Identity.Api.Models
{
    [ExcludeFromCodeCoverage]
    public class AccessTokenModel
    {
        public string JsonWebToken { get; set; }
    }
}