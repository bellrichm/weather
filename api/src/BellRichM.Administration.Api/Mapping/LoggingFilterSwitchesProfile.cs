using AutoMapper;
using BellRichM.Administration.Api.Models;
using BellRichM.Logging;

namespace BellRichM.Administration.Api.Mapping
{
    /// <summary>
    /// The role mapping profile.
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class LoggingFilterSwitchesProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingFilterSwitchesProfile"/> class.
        /// </summary>
        public LoggingFilterSwitchesProfile()
        {
            CreateMap<LoggingFilterSwitches, LoggingFilterSwitchesModel>();
            CreateMap<LoggingFilterSwitchesModel, LoggingFilterSwitches>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}