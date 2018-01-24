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
            CreateMap<User, UserModel>();
        }
    }
}