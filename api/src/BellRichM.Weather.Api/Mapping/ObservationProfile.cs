using AutoMapper;
using BellRichM.Weather.Api.Data;
using BellRichM.Weather.Api.Models;
using System;

namespace BellRichM.Weather.Api.Mapping
{
    /// <summary>
    /// The observation mapping profile.
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class ObservationProfile : Profile
    {
       /// <summary>
        /// Initializes a new instance of the <see cref="ObservationProfile"/> class.
        /// </summary>
        public ObservationProfile()
        {
            CreateMap<ObservationModel, Observation>()
                .ForMember(src => src.Year, opt => opt.Ignore())
                .ForMember(src => src.Month, opt => opt.Ignore())
                .ForMember(src => src.Day, opt => opt.Ignore())
                .ForMember(src => src.Hour, opt => opt.Ignore())
                .ForMember(src => src.Minute, opt => opt.Ignore());
        }
    }
}