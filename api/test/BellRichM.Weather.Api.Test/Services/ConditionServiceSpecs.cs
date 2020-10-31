using BellRichM.Helpers.Test;
using BellRichM.Logging;
using BellRichM.Weather.Api.Data;
using BellRichM.Weather.Api.Models;
using BellRichM.Weather.Api.Repositories;
using FluentAssertions;
using Machine.Specifications;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using It = Machine.Specifications.It;

namespace BellRichM.Weather.Api.Services.Test
{
    public class ConditionServiceSpecs
    {
        protected const int Offset = 0;
        protected const int Limit = 5;
        protected static LoggingData loggingData;
        protected static ConditionService conditionService;

        protected static Mock<ILoggerAdapter<ConditionService>> loggerMock;
        protected static Mock<IConditionRepository> conditionRepositoryMock;

        protected static TimePeriodModel timePeriodModel;

        protected static MinMaxConditionPage minMaxConditionPage;

        protected static IEnumerable<MinMaxCondition> minMaxConditions;

        protected static IEnumerable<Condition> conditions;
        protected static ConditionPage conditionPage;
        protected static MinMaxGroupPage minMaxGroupPage;
        protected static IEnumerable<MinMaxGroup> minMaxGroups;

        Establish context = () =>
        {
            // default to no logging
            loggingData = new LoggingData
            {
                EventLoggingData = new List<EventLoggingData>(),
                ErrorLoggingMessages = new List<string>()
            };

            loggerMock = new Mock<ILoggerAdapter<ConditionService>>();
            conditionRepositoryMock = new Mock<IConditionRepository>();
        };

        public static IEnumerable<MinMaxCondition> CreateMinMaxCondition()
        {
            minMaxConditions = new List<MinMaxCondition>
            {
                new MinMaxCondition
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

            return minMaxConditions;
        }

        public static IEnumerable<Condition> CreateCondition()
        {
            var conditions = new List<Condition>
            {
                new Condition
                {
                    Year = 2018,
                    Month = 9,
                    Day = 1,
                    Hour = 1,
                    WindGustDirection = 61.8725771445071,
                    WindGust = 4.00000994196379,
                    WindDirection = 59.8725771445071,
                    WindSpeed = 2.00000994196379,
                    OutsideTemperature = 67.2,
                    HeatIndex = 65.6,
                    Windchill = 83.0,
                    DewPoint = 60.8725771445071,
                    Barometer = 29.694,
                    RainRate = 0.0,
                    Rain = 4.00000994196379,
                    OutsideHumidity = 29.687
                }
            };

            return conditions.ToList();
        }
    }

    internal class When_creating_page_of_year_weather_conditions : ConditionServiceSpecs
    {
        Cleanup after = () =>
        {
        };

        Establish context = () =>
        {
            minMaxConditions = CreateMinMaxCondition();

            conditionRepositoryMock.Setup(x => x.GetYearCount()).Returns(Task.FromResult(minMaxConditions.Count()));
            conditionRepositoryMock.Setup(x => x.GetYear(Offset, Limit)).Returns(Task.FromResult(minMaxConditions));

            conditionService = new ConditionService(conditionRepositoryMock.Object);
        };

        Because of = () =>
        {
            minMaxConditionPage = conditionService.GetYearWeatherPage(Offset, Limit).Result;
        };

        Behaves_like<LoggingBehaviors<ConditionService>> correct_logging = () => { };

        It should_have_correct_total_count = () =>
        {
            minMaxConditionPage.Paging.TotalCount.Should().Equals(minMaxConditions.Count());
        };

        It should_have_correct_offset = () =>
        {
            minMaxConditionPage.Paging.Offset.Should().Equals(Offset);
        };

        It should_have_correct_limit = () =>
        {
            minMaxConditionPage.Paging.Limit.Should().Equals(Limit);
        };

        It should_have_correct_condition_data = () =>
        {
            minMaxConditionPage.MinMaxConditions.Should().BeEquivalentTo(minMaxConditions);
        };
    }

    internal class When_GetMinMaxConditionsByDay : ConditionServiceSpecs
    {
        Establish context = () =>
        {
            conditions = CreateCondition().ToList();
            var minMaxGroup = new MinMaxGroup
            {
                Month = conditions.First().Month,
                Day = conditions.First().Day
            };
            var minMaxGroupList = new List<MinMaxGroup>();
            minMaxGroupList.Add(minMaxGroup);
            minMaxGroups = minMaxGroupList;

            conditionRepositoryMock.Setup(x => x.GetMinMaxConditionsByDay(0, 0, Offset, Limit)).Returns(Task.FromResult(minMaxGroups));

            conditionService = new ConditionService(conditionRepositoryMock.Object);
        };

        Because of = () =>
            minMaxGroupPage = conditionService.GetMinMaxConditionsByDay(0, 0, Offset, Limit).Result;

        Behaves_like<LoggingBehaviors<ConditionService>> correct_logging = () => { };

        It should_have_correct_total_count = () =>
            minMaxGroupPage.Paging.TotalCount.Should().Equals(conditions.Count());

        It should_have_correct_offset = () =>
            minMaxGroupPage.Paging.Offset.Should().Equals(Offset);

        It should_have_correct_limit = () =>
            minMaxGroupPage.Paging.Limit.Should().Equals(Limit);

        It should_have_correct_condition_data = () =>
            minMaxGroupPage.MinMaxGroups.Should().BeEquivalentTo(minMaxGroups);
    }

    internal class When_GetConditionsByDay : ConditionServiceSpecs
    {
        Establish context = () =>
        {
            conditions = CreateCondition();

            conditionRepositoryMock.Setup(x => x.GetYearCount()).Returns(Task.FromResult(conditions.Count()));
            conditionRepositoryMock.Setup(x => x.GetConditionsByDay(Offset, Limit, timePeriodModel)).Returns(Task.FromResult(conditions));

            conditionService = new ConditionService(conditionRepositoryMock.Object);
        };

        Because of = () =>
            conditionPage = conditionService.GetConditionsByDay(Offset, Limit, timePeriodModel).Result;

        Behaves_like<LoggingBehaviors<ConditionService>> correct_logging = () => { };

        It should_have_correct_total_count = () =>
            conditionPage.Paging.TotalCount.Should().Equals(conditions.Count());

        It should_have_correct_offset = () =>
            conditionPage.Paging.Offset.Should().Equals(Offset);

        It should_have_correct_limit = () =>
            conditionPage.Paging.Limit.Should().Equals(Limit);

        It should_have_correct_condition_data = () =>
            conditionPage.Conditions.Should().BeEquivalentTo(conditions);
    }
}
