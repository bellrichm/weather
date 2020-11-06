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

        /// <inheritdoc/>
        public Task<MinMaxGroupPage> GetMinMaxConditionsByMinute(int startMinute, int endMinute, int offset, int limit)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public Task<MinMaxGroupPage> GetMinMaxConditionsByHour(int startHour, int endHour, int offset, int limit)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<MinMaxGroupPage> GetMinMaxConditionsByDay(int startDayOfYear, int endDayOfYear, int offset, int limit)
        {
            var minMaxGroups = await _conditionRepository.GetMinMaxConditionsByDay(startDayOfYear, endDayOfYear, offset, limit).ConfigureAwait(true);

            var paging = new Paging
            {
                TotalCount = 366,
                Offset = offset,
                Limit = limit
            };

            var minMaxGroupPage = new MinMaxGroupPage
            {
                Paging = paging,
                MinMaxGroups = minMaxGroups
            };

            return minMaxGroupPage;
        }

        /// <inheritdoc/>
        public Task<MinMaxGroupPage> GetMinMaxConditionsByWeek(int startWeekOfYear, int endWeekOfYear, int offset, int limit)
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>
        public async Task<ConditionPage> GetConditionsByDay(int offset, int limit, TimePeriodModel timePeriodModel)
        {
            var conditions = await _conditionRepository.GetConditionsByDay(offset, limit, timePeriodModel).ConfigureAwait(true);
            var paging = new Paging
            {
                TotalCount = await _conditionRepository.GetDayCount().ConfigureAwait(true),
                Offset = 0,
                Limit = 10000
            };

            var condition2Page = new ConditionPage
            {
                Paging = paging,
                Conditions = conditions
            };

            return condition2Page;
        }
    }
}