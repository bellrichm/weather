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
            CreateMap<Observation, ObservationModel>();
            CreateMap<ObservationModel, Observation>()
                .ForMember(dest => dest.Year, opt => opt.Ignore())
                .ForMember(dest => dest.Month, opt => opt.Ignore())
                .ForMember(dest => dest.Day, opt => opt.Ignore())
                .ForMember(dest => dest.Hour, opt => opt.Ignore())
                .ForMember(dest => dest.Minute, opt => opt.Ignore())
                .AfterMap((src, dest) =>
                {
                    var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc); // UTC vs Eastern...???
                    var dateTime = epoch.AddSeconds(src.DateTime);
                    dest.Year = dateTime.Year;
                    dest.Month = dateTime.Month;
                    dest.Day = dateTime.Day;
                    dest.Hour = dateTime.Hour;
                    dest.Minute = dateTime.Minute;
                });
        }
    }
}