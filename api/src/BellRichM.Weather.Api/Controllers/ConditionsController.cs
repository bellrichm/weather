using System;
using System.Threading.Tasks;
using AutoMapper;
using BellRichM.Api.Controllers;
using BellRichM.Logging;
using BellRichM.Weather.Api.Filters;
using BellRichM.Weather.Api.Models;
using BellRichM.Weather.Api.Services;
using Microsoft.AspNetCore.Mvc;

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
        /// <returns>The <see cref="MinMaxConditionPageModel"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        /// <remarks>Not yet implemented.</remarks>
        [ValidateConditionLimit]
        [HttpGet("/api/[controller]/years", Name="GetYearsConditionPage")]
        public async Task<IActionResult> GetYearsConditionPage([FromQuery] int offset, [FromQuery] int limit)
        {
            _logger.LogEvent(EventId.ConditionsController_GetYearsConditionPage, "{@offset} {@limit}", offset, limit);
            if (!ModelState.IsValid)
            {
                _logger.LogDiagnosticInformation("{@ModelState}", ModelState);
                var errorResponseModel = CreateModel();
                return BadRequest(errorResponseModel);
            }

            var minMaxConditionPage = await _conditionService.GetYearWeatherPage(offset, limit).ConfigureAwait(true);
            var minMaxConditionPageModel = _mapper.Map<MinMaxConditionPageModel>(minMaxConditionPage);
            minMaxConditionPageModel.Links = GetNavigationLinks("GetYearsConditionPage", minMaxConditionPageModel.Paging);
            return Ok(minMaxConditionPageModel);
        }

        /// <summary>
        /// Gets the minimum and maximum weather conditions for the year.
        /// </summary>
        /// <param name="year">The year to get the conditions for.</param>
        /// <returns>The <see cref="MinMaxConditionModel"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        /// <remarks>Not yet implemented.</remarks>
        [HttpGet("/api/[controller]/years/{year}", Name="GetYearDetail")]
        public async Task<IActionResult> GetYearDetail([FromRoute] int year)
        {
            _logger.LogEvent(EventId.ConditionsController_GetYearDetail, "{@year}", year);

            // This is to get rid of warning CS1998, remove when implementing this method.
            await Task.CompletedTask.ConfigureAwait(true);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the monthly mininimum and maximum weather conditions for the year.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="offset">The starting offset.</param>
        /// <param name="limit">The maximum number of years to return.</param>
        /// <returns>The <see cref="MinMaxConditionPageModel"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        [HttpGet("/api/[controller]/years/{year}/months", Name="GetMonthsConditionPage")]
        public async Task<IActionResult> GetMonthsConditionPage([FromRoute] int year, [FromQuery] int offset, [FromQuery] int limit)
        {
            _logger.LogEvent(EventId.ConditionsController_GetMonthsConditionPage, "{@year} {@offset} {@limit}", year, offset, limit);

            // This is to get rid of warning CS1998, remove when implementing this method.
            await Task.CompletedTask.ConfigureAwait(true);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the minimum and maximum weather conditions for the year and month.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <returns>The <see cref="MinMaxConditionModel"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        [HttpGet("/api/[controller]/years/{year}/months/{month}", Name="GetMonthDetail")]
        public async Task<IActionResult> GetMonthDetail([FromRoute] int year, [FromRoute] int month)
        {
            _logger.LogEvent(EventId.ConditionsController_GetMonthDetail, "{@year} {@month}", year, month);

            // This is to get rid of warning CS1998, remove when implementing this method.
            await Task.CompletedTask.ConfigureAwait(true);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the daily mininimum and maximum weather conditions for the year and month.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="offset">The starting offset.</param>
        /// <param name="limit">The maximum number of years to return.</param>
        /// <returns>The <see cref="MinMaxConditionPageModel"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        [HttpGet("/api/[controller]/years/{year}/months/{month}/days", Name="GetDaysConditionPage")]
        public async Task<IActionResult> GetDaysConditionPage([FromRoute] int year, [FromRoute] int month, [FromQuery] int offset, [FromQuery] int limit)
        {
            _logger.LogEvent(EventId.ConditionsController_GetDaysConditionPage, "{@year} {@month} {@offset} {@limit}", year, month, offset, limit);

            // This is to get rid of warning CS1998, remove when implementing this method.
            await Task.CompletedTask.ConfigureAwait(true);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the minimum and maximum weather conditions for the year, month, and day.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <returns>The <see cref="MinMaxConditionModel"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        [HttpGet("/api/[controller]/years/{year}/months/{month}/days/{day}", Name="GetDayDetail")]
        public async Task<IActionResult> GetDayDetail([FromRoute] int year, [FromRoute] int month, [FromRoute] int day)
        {
            _logger.LogEvent(EventId.ConditionsController_GetDayDetail, "{@year} {@month} {@day}", year, month, day);

            // This is to get rid of warning CS1998, remove when implementing this method.
            await Task.CompletedTask.ConfigureAwait(true);
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
        /// <returns>The <see cref="MinMaxConditionPageModel"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        [HttpGet("/api/[controller]/years/{year}/months/{month}/days/{day}/hours", Name="GetHoursConditionPage")]
        public async Task<IActionResult> GetHoursConditionPage([FromRoute] int year, [FromRoute] int month, [FromRoute] int day, [FromQuery] int offset, [FromQuery] int limit)
        {
            _logger.LogEvent(EventId.ConditionsController_GetHoursConditionPage, "{@year} {@month} {@day} {@offset} {@limit}", year, month, day, offset, limit);

            // This is to get rid of warning CS1998, remove when implementing this method.
            await Task.CompletedTask.ConfigureAwait(true);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the minimum and maximum weather conditions for the year, month, day, and hour.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <param name="hour">The hour.</param>
        /// <returns>The <see cref="MinMaxConditionModel"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        [HttpGet("/api/[controller]/years/{year}/months/{month}/days/{day}/hours/{hour}", Name="GetHourDetail")]
        public async Task<IActionResult> GetHourDetail([FromRoute] int year, [FromRoute] int month, [FromRoute] int day, [FromRoute] int hour)
        {
            _logger.LogEvent(EventId.ConditionsController_GetHourDetail, "{@year} {@month} {@day} {@hour}", year, month, day, hour);

            // This is to get rid of warning CS1998, remove when implementing this method.
            await Task.CompletedTask.ConfigureAwait(true);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets minimum and maximimum weather conditions for the month across all years.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <param name="offset">The starting offset.</param>
        /// <param name="limit">The maximum number of years to return.</param>
        /// <returns>The <see cref="MinMaxConditionPageModel"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        [HttpGet("/api/[controller]/years/months/{month}", Name="GetYearsMonthConditionPage")]
        public async Task<IActionResult> GetYearsMonthConditionPage([FromRoute] int month, [FromQuery] int offset, [FromQuery] int limit)
        {
            _logger.LogEvent(EventId.ConditionsController_GetYearsMonthConditionPage, "{@month} {@offset} {@limit}", month, offset, limit);

            // This is to get rid of warning CS1998, remove when implementing this method.
            await Task.CompletedTask.ConfigureAwait(true);
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets minimum and maximimum weather conditions for the month and day across all years.
        /// </summary>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <param name="offset">The starting offset.</param>
        /// <param name="limit">The maximum number of years to return.</param>
        /// <returns>The <see cref="MinMaxConditionPageModel"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        [HttpGet("/api/[controller]/years/months/{month}/days/{day}", Name="GetYearsDayConditionPage")]
        public async Task<IActionResult> GetYearsDayConditionPage([FromRoute] int month, [FromRoute] int day, [FromQuery] int offset, [FromQuery] int limit)
        {
            _logger.LogEvent(EventId.ConditionsController_GetYearsDayConditionPage, "{@month} {@day} {@offset} {@limit}", month, day, offset, limit);

            // This is to get rid of warning CS1998, remove when implementing this method.
            await Task.CompletedTask.ConfigureAwait(true);
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
        /// <returns>The <see cref="MinMaxConditionPageModel"/>.</returns>
        /// <exception cref="NotImplementedException">Not implemented.</exception>
        [HttpGet("/api/[controller]/years/months/{month}/days/{day}/hours/{hour}", Name="GetYearsHourConditionPage")]
        public async Task<IActionResult> GetYearsHourConditionPage([FromRoute] int month, [FromRoute] int day, [FromRoute] int hour, [FromQuery] int offset, [FromQuery] int limit)
        {
            _logger.LogEvent(EventId.ConditionsController_GetYearsHourConditionPage, "{@month} {@day} {@hour} {@offset} {@limit}", month, day, hour, offset, limit);

            // This is to get rid of warning CS1998, remove when implementing this method.
            await Task.CompletedTask.ConfigureAwait(true);
            throw new NotImplementedException();
        }
    }
}
