using AutoMapper;
using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Models;

namespace BellRichM.Identity.Api.Mapping
{
    /// <summary>
    /// The user mapping profile.
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class UserProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserProfile"/> class.
        /// </summary>
        public UserProfile()
        {
            CreateMap<ClaimValue, ClaimValueModel>();
            CreateMap<ClaimValueModel, ClaimValue>();
            CreateMap<Role, RoleModel>();
            CreateMap<RoleModel, Role>()
                .ForMember(dest => dest.NormalizedName, dest => dest.Ignore())
                .ForMember(dest => dest.ConcurrencyStamp, dest => dest.Ignore());
            CreateMap<User, UserModel>();
            CreateMap<UserCreateModel, User>()
                .ForMember(dest => dest.NormalizedUserName, dest => dest.Ignore())
                .ForMember(dest => dest.NormalizedEmail, dest => dest.Ignore())
                .ForMember(dest => dest.EmailConfirmed, dest => dest.Ignore())
                .ForMember(dest => dest.PasswordHash, dest => dest.Ignore())
                .ForMember(dest => dest.SecurityStamp, dest => dest.Ignore())
                .ForMember(dest => dest.ConcurrencyStamp, dest => dest.Ignore())
                .ForMember(dest => dest.PhoneNumberConfirmed, dest => dest.Ignore())
                .ForMember(dest => dest.LockoutEnd, dest => dest.Ignore())
                .ForMember(dest => dest.AccessFailedCount, dest => dest.Ignore())
               .ForMember(src => src.Id, opt => opt.Ignore());
        }
    }
}