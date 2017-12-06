using AutoMapper;
using BellRichM.Identity.Api.Data;
using BellRichM.Identity.Api.Models;

namespace BellRichM.Identity.Api.Mapping
{
    public class ClaimValueProfile : Profile
    {
        public ClaimValueProfile ()
        {
            CreateMap<ClaimValue, ClaimValueModel>();
        }
    }
}