using BellRichM.Service.Data;
using BellRichM.Weather.Api.Data;
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
        public async Task<ConditionPage> GetYearWeatherPage(int offset, int limit)
        {
            var conditions = await _conditionRepository.GetYear(offset, limit).ConfigureAwait(true);

            var paging = new Paging
            {
                TotalCount = await _conditionRepository.GetYearCount().ConfigureAwait(true),
                Offset = offset,
                Limit = limit
            };

            var conditionPage = new ConditionPage
            {
                Paging = paging,
                MinMaxConditions = conditions
            };

            return conditionPage;
        }
    }
}