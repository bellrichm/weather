using BellRichM.Weather.Api.Data;
using BellRichM.Weather.Api.Repositories;

namespace BellRichM.Weather.Api.Services
{
    /// <inheritdoc/>
    public class WeatherService : IWeatherService
    {
        private readonly IWeatherRepository _weatherRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherService"/> class.
        /// </summary>
        /// <param name="weatherRepository">The <see cref="IWeatherRepository"/>.</param>
        public WeatherService(IWeatherRepository weatherRepository)
        {
            _weatherRepository = weatherRepository;
        }

        /// <inheritdoc/>
        public ConditionPage GetYearWeatherPage(int offset, int limit)
        {
            var conditions = _weatherRepository.GetYear(offset, limit);
            var conditionPage = new ConditionPage
            {
                TotalCount = _weatherRepository.GetYearCount(),
                Offset = offset,
                Limit = limit,
                Conditions = conditions
            };

            return conditionPage;
        }
    }
}