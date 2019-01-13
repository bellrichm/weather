using System;
using System.Collections.Generic;
using AutoMapper;
using BellRichM.Api.Controllers;
using BellRichM.Logging;
using BellRichM.Weather.Api.Data;
using BellRichM.Weather.Api.Models;
using BellRichM.Weather.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;

namespace BellRichM.Weather.Api.Controllers
{
    /// <summary>
    /// The conditions controller.
    /// </summary>
    /// <seealso cref="Controller" />
    [Route("api/[controller]")]
    public class ConditionsController : ApiController
    {
        private readonly ILoggerAdapter<ConditionsController> _logger;
        private readonly IMapper _mapper;
        private readonly IWeatherService _weatherService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionsController"/> class.
        /// </summary>
        /// <param name="logger">The <see cref="ILoggerAdapter{T}"/>.</param>
        /// <param name="mapper">The <see cref="IMapper"/>.</param>
        /// <param name="weatherService">The <see cref="IWeatherService"/>.</param>
        public ConditionsController(ILoggerAdapter<ConditionsController> logger, IMapper mapper, IWeatherService weatherService)
        {
            _logger = logger;
            _mapper = mapper;
            _weatherService = weatherService;
        }

        /// <summary>
        /// Gets the yearly minimum and maximimum weather conditions.
        /// </summary>
        /// <param name="offset">The starting offset.</param>
        /// <param name="limit">The maximum number of years to return.</param>
        /// <returns>The <see cref="ConditionPageModel"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        /// <remarks>Not yet implemented.</remarks>
        [HttpGet("/api/[controller]/years", Name="GetYearsConditionPage")]
        public ConditionPageModel GetYearsConditionPage([FromQuery] int offset, [FromQuery] int limit)
        {
            var routeName = "GetYearsConditionPage";
            _logger.LogDiagnosticInformation("{GetYearsConditionPage} called.", routeName);

            var conditionPage = _weatherService.GetYearWeatherPage(offset, limit);
            var conditionPageModel = _mapper.Map<ConditionPageModel>(conditionPage);
            conditionPageModel.Links = GetNavigationLinks(routeName, conditionPageModel.Paging);
            return conditionPageModel;
        }

        /// <summary>
        /// Gets the minimum and maximum weather conditions for the year.
        /// </summary>
        /// <param name="year">The year to get the conditions for.</param>
        /// <returns>The <see cref="ConditionModel"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        /// <remarks>Not yet implemented.</remarks>
        [HttpGet("/api/[controller]/years/{year}", Name="GetYearDetail")]
        public ConditionModel GetYearDetail([FromRoute] int year)
        {
            _logger.LogDiagnosticInformation("GetYearDetail called with {year}.", year);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the monthly mininimum and maximum weather conditions for the year.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <returns>The <see cref="ConditionPageModel"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        [HttpGet("/api/[controller]/years/{year}/months", Name="GetMonthsConditionPage")]
        public ConditionPageModel GetMonthsConditionPage([FromRoute] int year)
        {
            _logger.LogDiagnosticInformation("GetMonthsConditionPage called with {year}.", year);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the minimum and maximum weather conditions for the year and month.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <returns>The <see cref="ConditionModel"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        [HttpGet("/api/[controller]/years/{year}/months/{month}", Name="GetMonthDetail")]
        public ConditionModel GetMonthDetail([FromRoute] int year, [FromRoute] int month)
        {
            _logger.LogDiagnosticInformation("GetMonthDetail called with {year} {month}.", year, month);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the daily mininimum and maximum weather conditions for the year and month.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <returns>The <see cref="ConditionPageModel"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        [HttpGet("/api/[controller]/years/{year}/months/{month}/days", Name="GetDaysConditionPage")]
        public ConditionPageModel GetDaysConditionPage([FromRoute] int year, [FromRoute] int month)
        {
            _logger.LogDiagnosticInformation("GetDaysConditionPage called with {year} {month}.", year, month);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the minimum and maximum weather conditions for the year, month, and day.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <returns>The <see cref="ConditionModel"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        [HttpGet("/api/[controller]/years/{year}/months/{month}/days/{day}", Name="GetDayDetail")]
        public ConditionModel GetDayDetail([FromRoute] int year, [FromRoute] int month, [FromRoute] int day)
        {
            _logger.LogDiagnosticInformation("GetDayDetail called with {year} {month} {day}.", year, month, day);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the hourly mininimum and maximum weather conditions for the year, month, and day.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <returns>The <see cref="ConditionPageModel"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        [HttpGet("/api/[controller]/years/{year}/months/{month}/days/{day}/hours", Name="GetHoursConditionPage")]
        public ConditionPageModel GetHoursConditionPage([FromRoute] int year, [FromRoute] int month, [FromRoute] int day)
        {
            _logger.LogDiagnosticInformation("GetHoursConditionPage called with {year} {month} {day}.", year, month, day);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the minimum and maximum weather conditions for the year, month, day, and hour.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <param name="hour">The hour.</param>
        /// <returns>The <see cref="ConditionModel"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        [HttpGet("/api/[controller]/years/{year}/months/{month}/days/{day}/hours/{hour}", Name="GetHourDetail")]
        public ConditionModel GetHourDetail([FromRoute] int year, [FromRoute] int month, [FromRoute] int day, [FromRoute] int hour)
        {
            _logger.LogDiagnosticInformation("GetHourDetail called with {year} {month} {day} {hour}.", year, month, day, hour);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets minimum and maximimum weather conditions for the month across all years.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <returns>The <see cref="ConditionPageModel"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        [HttpGet("/api/[controller]/years/months/{month}", Name="GetYearsMonthConditionPage")]
        public ConditionPageModel GetYearsMonthConditionPage([FromRoute] int month)
        {
            _logger.LogDiagnosticInformation("GetYearsMonthConditionPage called with {month}.", month);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets minimum and maximimum weather conditions for the month and day across all years.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <returns>The <see cref="ConditionPageModel"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        [HttpGet("/api/[controller]/years/months/{month}/days/{day}", Name="GetYearsDayConditionPage")]
        public ConditionPageModel GetYearsDayConditionPage([FromRoute] int month, [FromRoute] int day)
        {
            _logger.LogDiagnosticInformation("GetYearsDayConditionPage called with {month} {day}.", month, day);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets minimum and maximimum weather conditions for the month, day and hour across all years.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <param name="hour">The hour.</param>
        /// <returns>The <see cref="ConditionPageModel"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        [HttpGet("/api/[controller]/years/months/{month}/days/{day}/hours/{hour}", Name="GetYearsHourConditionPage")]
        public ConditionPageModel GetYearsHourConditionPage([FromRoute] int month, [FromRoute] int day, [FromRoute] int hour)
        {
            _logger.LogDiagnosticInformation("GetYearsHourConditionPage called with {month} {day} {hour}.", month, day, hour);

            throw new NotImplementedException();
        }
    }
}