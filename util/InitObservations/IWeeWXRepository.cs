using BellRichM.Weather.Api.Data;
using BellRichM.Weather.Api.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace InitObservations
{
    /// <summary>
    /// The WeeWX repository.
    /// </summary>
    public interface IWeeWXRepository
    {
        /// <summary>
        /// Get it.
        /// </summary>
        void Get();
    }
}
