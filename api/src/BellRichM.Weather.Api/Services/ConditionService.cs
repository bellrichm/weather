using BellRichM.Service.Data;
using BellRichM.Weather.Api.Data;
using BellRichM.Weather.Api.Repositories;

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
        public ConditionPage GetYearWeatherPage(int offset, int limit)
        {
            var conditions = _conditionRepository.GetYear(offset, limit);

            var paging = new Paging
            {
                TotalCount = _conditionRepository.GetYearCount(),
                Offset = offset,
                Limit = limit
            };

            var conditionPage = new ConditionPage
            {
                Paging = paging,
                Conditions = conditions
            };

            return conditionPage;
        }
    }
}