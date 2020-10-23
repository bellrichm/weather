using BellRichM.Service.Data;
using BellRichM.Weather.Api.Data;
using BellRichM.Weather.Api.Models;
using BellRichM.Weather.Api.Repositories;
using System.Threading.Tasks;

namespace BellRichM.Weather.Api.Services
{
    /// <inheritdoc/>
    public class ConditionService : IConditionService
    {
        private readonly IConditionRepository _conditionRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionService"/> class.
        /// </summary>
        /// <param name="conditionRepository">The <see cref="IConditionRepository"/>.</param>
        public ConditionService(IConditionRepository conditionRepository)
        {
            _conditionRepository = conditionRepository;
        }

        /// <inheritdoc/>
        public Task<ConditionPage> GetConditionsByDay(int offset, int limit, TimePeriodModel timePeriodModel)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<MinMaxConditionPage> GetYearWeatherPage(int offset, int limit)
        {
            var minMaxConditions = await _conditionRepository.GetYear(offset, limit).ConfigureAwait(true);

            var paging = new Paging
            {
                TotalCount = await _conditionRepository.GetYearCount().ConfigureAwait(true),
                Offset = offset,
                Limit = limit
            };

            var minMaxConditionPage = new MinMaxConditionPage
            {
                Paging = paging,
                MinMaxConditions = minMaxConditions
            };

            return minMaxConditionPage;
        }
    }
}