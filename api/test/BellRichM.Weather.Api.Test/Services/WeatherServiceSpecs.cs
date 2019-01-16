using BellRichM.Helpers.Test;
using BellRichM.Logging;
using BellRichM.Weather.Api.Data;
using BellRichM.Weather.Api.Repositories;
using BellRichM.Weather.Api.Services;
using FluentAssertions;
using Machine.Specifications;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using IT = Moq.It;
using It = Machine.Specifications.It;

namespace BellRichM.Weather.Api.Services.Test
{
    public class WeatherServiceSpecs
    {
        protected const int Offset = 0;
        protected const int Limit = 5;
        protected static LoggingData loggingData;

        protected static Mock<ILoggerAdapter<WeatherService>> loggerMock;
        protected static Mock<IWeatherRepository> weatherRepositoryMock;

        protected static WeatherService weatherService;
        protected static ConditionPage conditionPage;

        protected static List<Condition> conditions;

        Establish context = () =>
        {
            // default to no logging
            loggingData = new LoggingData
            {
                EventLoggingData = new List<EventLoggingData>(),
                ErrorLoggingMessages = new List<string>()
            };

            conditions = new List<Condition>
            {
                new Condition
                {
                    Year = 2018,
                    Month = 9,
                    Day = 1,
                    Hour = 1,
                    MaxTemp = "67.2",
                    MinTemp = "65.6",
                    MaxHumidity = "83.0",
                    MinHumidity = "80.0",
                    MaxDewpoint = "60.8725771445071",
                    MinDewpoint = "60.0932637870109",
                    MaxHeatIndex = "67.2",
                    MinWindchill = "65.6",
                    MaxBarometer = "29.694",
                    MinBarometer = "29.687",
                    MaxET = "0.001",
                    MinET = "0.0",
                    MaxUV = "0.0",
                    MinUV = "0.0",
                    MaxRadiation = "0.0",
                    MinRadiation = "0.0",
                    MaxRainRate = "0.0",
                    MaxWindGust = "4.00000994196379"
                }
            };

            loggerMock = new Mock<ILoggerAdapter<WeatherService>>();
            weatherRepositoryMock = new Mock<IWeatherRepository>();

            weatherRepositoryMock.Setup(x => x.GetYearCount()).Returns(conditions.Count);
            weatherRepositoryMock.Setup(x => x.GetYear(Offset, Limit)).Returns(conditions);

            weatherService = new WeatherService(weatherRepositoryMock.Object);
        };
    }

    internal class When_creating_page_of_year_weather_conditions : WeatherServiceSpecs
    {
        Cleanup after = () =>
        {
        };

        Establish context = () =>
        {
        };

        Because of = () =>
        {
            conditionPage = weatherService.GetYearWeatherPage(Offset, Limit);
        };

#pragma warning disable 169
        Behaves_like<LoggingBehaviors<WeatherService>> correct_logging;
#pragma warning restore 169

        It should_have_correct_total_count = () =>
        {
            conditionPage.Paging.TotalCount.Should().Equals(conditions.Count);
        };

        It should_have_correct_offset = () =>
        {
            conditionPage.Paging.Offset.Should().Equals(Offset);
        };

        It should_have_correct_limit = () =>
        {
            conditionPage.Paging.Limit.Should().Equals(Limit);
        };
        It should_have_correct_condition_data = () =>
        {
            conditionPage.Conditions.Should().BeEquivalentTo(conditions);
        };
    }
}
