using AutoMapper;
using BellRichM.Weather.Api.Data;
using BellRichM.Weather.Api.Models;

namespace BellRichM.Weather.Api.Mapping
{
    /// <summary>
    /// The observation mapping profile.
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class ObservationDateTimeProfile : Profile
    {
       /// <summary>
        /// Initializes a new instance of the <see cref="ObservationDateTimeProfile"/> class.
        /// </summary>
        public ObservationDateTimeProfile()
        {
            CreateMap<ObservationDateTime, ObservationDateTimeModel>();
        }
    }
}
