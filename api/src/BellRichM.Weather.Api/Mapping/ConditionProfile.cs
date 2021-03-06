using AutoMapper;
using BellRichM.Api.Models;
using BellRichM.Service.Data;
using BellRichM.Weather.Api.Data;
using BellRichM.Weather.Api.Models;

namespace BellRichM.Weather.Api.Mapping
{
    /// <summary>
    /// The user mapping profile.
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class ConditionProfile : Profile
    {
       /// <summary>
        /// Initializes a new instance of the <see cref="ConditionProfile"/> class.
        /// </summary>
        public ConditionProfile()
        {
            CreateMap<Paging, PagingModel>();
            CreateMap<MinMaxCondition, MinMaxConditionModel>();
            CreateMap<MinMaxConditionPage, MinMaxConditionPageModel>()
                .ForMember(dest => dest.Links, dest => dest.Ignore());
            CreateMap<MinMaxGroup, MinMaxGroupModel>();
            CreateMap<MinMaxGroupPage, MinMaxGroupPageModel>()
                .ForMember(dest => dest.Links, dest => dest.Ignore());
            CreateMap<Condition, ConditionModel>();
            CreateMap<ConditionPage, ConditionPageModel>()
                .ForMember(dest => dest.Links, dest => dest.Ignore());
        }
    }
}
