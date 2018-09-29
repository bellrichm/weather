using AutoMapper;
using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Models;

namespace BellRichM.Identity.Api.Mapping
{
    /// <summary>
    /// The role mapping profile.
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class RoleProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoleProfile"/> class.
        /// </summary>
        public RoleProfile()
        {
            CreateMap<Role, RoleModel>();
            CreateMap<RoleModel, Role>()
                .ForMember(dest => dest.NormalizedName, dest => dest.Ignore())
                .ForMember(dest => dest.ConcurrencyStamp, dest => dest.Ignore());
        }
    }
}