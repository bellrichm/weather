using AutoMapper;
using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Models;

namespace BellRichM.Identity.Api.Mapping
{
    /// <summary>
    /// The claimvalue mapping profile.
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class ClaimValueProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ClaimValueProfile"/> class.
        /// </summary>
        public ClaimValueProfile()
        {
            CreateMap<ClaimValue, ClaimValueModel>();
        }
    }
}