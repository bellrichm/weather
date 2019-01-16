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
        private readonly IConditionService _conditionService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionsController"/> class.
        /// </summary>
        /// <param name="logger">The <see cref="ILoggerAdapter{T}"/>.</param>
        /// <param name="mapper">The <see cref="IMapper"/>.</param>
        /// <param name="conditionService">The <see cref="IConditionService"/>.</param>
        public ConditionsController(ILoggerAdapter<ConditionsController> logger, IMapper mapper, IConditionService conditionService)
        {
            _logger = logger;
            _mapper = mapper;
            _conditionService = conditionService;
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
            _logger.LogEvent(EventId.ConditionsController_GetYearsConditionPage, "{@offset} {@limit}", offset, limit);

            var conditionPage = _conditionService.GetYearWeatherPage(offset, limit);
            var conditionPageModel = _mapper.Map<ConditionPageModel>(conditionPage);
            conditionPageModel.Links = GetNavigationLinks("GetYearsConditionPage", conditionPageModel.Paging);
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
            _logger.LogEvent(EventId.ConditionsController_GetYearDetail, "{@year}", year);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the monthly mininimum and maximum weather conditions for the year.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="offset">The starting offset.</param>
        /// <param name="limit">The maximum number of years to return.</param>
        /// <returns>The <see cref="ConditionPageModel"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        [HttpGet("/api/[controller]/years/{year}/months", Name="GetMonthsConditionPage")]
        public ConditionPageModel GetMonthsConditionPage([FromRoute] int year, [FromQuery] int offset, [FromQuery] int limit)
        {
            _logger.LogEvent(EventId.ConditionsController_GetMonthsConditionPage, "{@year} {@offset} {@limit}", year, offset, limit);

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
            _logger.LogEvent(EventId.ConditionsController_GetMonthDetail, "{@year} {@month}", year, month);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the daily mininimum and maximum weather conditions for the year and month.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="offset">The starting offset.</param>
        /// <param name="limit">The maximum number of years to return.</param>
        /// <returns>The <see cref="ConditionPageModel"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        [HttpGet("/api/[controller]/years/{year}/months/{month}/days", Name="GetDaysConditionPage")]
        public ConditionPageModel GetDaysConditionPage([FromRoute] int year, [FromRoute] int month, [FromQuery] int offset, [FromQuery] int limit)
        {
            _logger.LogEvent(EventId.ConditionsController_GetDaysConditionPage, "{@year} {@month} {@offset} {@limit}", year, month, offset, limit);

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
            _logger.LogEvent(EventId.ConditionsController_GetDayDetail, "{@year} {@month} {@day}", year, month, day);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the hourly mininimum and maximum weather conditions for the year, month, and day.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <param name="offset">The starting offset.</param>
        /// <param name="limit">The maximum number of years to return.</param>
        /// <returns>The <see cref="ConditionPageModel"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        [HttpGet("/api/[controller]/years/{year}/months/{month}/days/{day}/hours", Name="GetHoursConditionPage")]
        public ConditionPageModel GetHoursConditionPage([FromRoute] int year, [FromRoute] int month, [FromRoute] int day, [FromQuery] int offset, [FromQuery] int limit)
        {
            _logger.LogEvent(EventId.ConditionsController_GetHoursConditionPage, "{@year} {@month} {@day} {@offset} {@limit}", year, month, day, offset, limit);

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
            _logger.LogEvent(EventId.ConditionsController_GetHourDetail, "{@year} {@month} {@day} {@hour}", year, month, day, hour);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets minimum and maximimum weather conditions for the month across all years.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <param name="offset">The starting offset.</param>
        /// <param name="limit">The maximum number of years to return.</param>
        /// <returns>The <see cref="ConditionPageModel"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        [HttpGet("/api/[controller]/years/months/{month}", Name="GetYearsMonthConditionPage")]
        public ConditionPageModel GetYearsMonthConditionPage([FromRoute] int month, [FromQuery] int offset, [FromQuery] int limit)
        {
            _logger.LogEvent(EventId.ConditionsController_GetYearsMonthConditionPage, "{@month} {@offset} {@limit}", month, offset, limit);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets minimum and maximimum weather conditions for the month and day across all years.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <param name="offset">The starting offset.</param>
        /// <param name="limit">The maximum number of years to return.</param>
        /// <returns>The <see cref="ConditionPageModel"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        [HttpGet("/api/[controller]/years/months/{month}/days/{day}", Name="GetYearsDayConditionPage")]
        public ConditionPageModel GetYearsDayConditionPage([FromRoute] int month, [FromRoute] int day, [FromQuery] int offset, [FromQuery] int limit)
        {
            _logger.LogEvent(EventId.ConditionsController_GetYearsDayConditionPage, "{@month} {@day} {@offset} {@limit}", month, day, offset, limit);

            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets minimum and maximimum weather conditions for the month, day and hour across all years.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <param name="hour">The hour.</param>
        /// <param name="offset">The starting offset.</param>
        /// <param name="limit">The maximum number of years to return.</param>
        /// <returns>The <see cref="ConditionPageModel"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        [HttpGet("/api/[controller]/years/months/{month}/days/{day}/hours/{hour}", Name="GetYearsHourConditionPage")]
        public ConditionPageModel GetYearsHourConditionPage([FromRoute] int month, [FromRoute] int day, [FromRoute] int hour, [FromQuery] int offset, [FromQuery] int limit)
        {
            _logger.LogEvent(EventId.ConditionsController_GetYearsHourConditionPage, "{@month} {@day} {@hour} {@offset} {@limit}", month, day, hour, offset, limit);

            throw new NotImplementedException();
        }
    }
}