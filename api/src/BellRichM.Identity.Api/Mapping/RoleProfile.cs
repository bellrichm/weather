using AutoMapper;
using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Models;

namespace BellRichM.Identity.Api.Mapping
{
    public class RoleProfile : Profile
    {
        public RoleProfile()
        {
            CreateMap<Role, RoleModel>();
        }
    }
}