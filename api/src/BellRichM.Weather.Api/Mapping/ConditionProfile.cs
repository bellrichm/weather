using AutoMapper;
using BellRichM.Weather.Api.Data;
using BellRichM.Weather.Api.Models;
using System;

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
            CreateMap<ConditionPage, ConditionPageModel>()
                .ForMember(dest => dest.Links, dest => dest.Ignore());
        }
    }
}