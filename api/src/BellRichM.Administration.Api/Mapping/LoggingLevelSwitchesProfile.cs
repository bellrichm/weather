using AutoMapper;
using BellRichM.Administration.Api.Models;
using BellRichM.Logging;
using Serilog.Core;

namespace BellRichM.Administration.Api.Mapping
{
    /// <summary>
    /// The role mapping profile.
    /// </summary>
    /// <seealso cref="AutoMapper.Profile" />
    public class LoggingLevelSwitchesProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingLevelSwitchesProfile"/> class.
        /// </summary>
        public LoggingLevelSwitchesProfile()
        {
            CreateMap<LoggingLevelSwitch, LoggingLevelSwitchModel>();
            CreateMap<LoggingLevelSwitchModel, LoggingLevelSwitch>();
            CreateMap<LoggingLevelSwitches, LoggingLevelSwitchesModel>();
            CreateMap<LoggingLevelSwitchesModel, LoggingLevelSwitches>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}