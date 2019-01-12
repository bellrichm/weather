using BellRichM.Logging;
using BellRichM.Weather.Api.Configuration;
using BellRichM.Weather.Api.Data;
using BellRichM.Weather.Api.Repositories;
using FluentAssertions;
using Machine.Specifications;
using Microsoft.Data.Sqlite;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data.Common;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using IT = Moq.It;
using It = Machine.Specifications.It;

namespace BellRichM
{
    internal class WeatherRepositorySpecs
    {
        protected const int Offset = 0;
        protected const int Limit = 5;

        protected static Condition testCondition;
        protected static Mock<ILoggerAdapter<WeatherRepository>> loggerMock;
        protected static WeatherRepository weatherRepository;
        protected static WeatherRepositoryDbProviderFactory weatherRepositoryDbProviderFactory;
        protected static WeatherRepositoryConfiguration weatherRepositoryConfiguration;

        Establish context = () =>
        {
            testCondition = new Condition
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
            };

            var dbProviderFactory = SqliteFactory.Instance;
            weatherRepositoryDbProviderFactory = new WeatherRepositoryDbProviderFactory(dbProviderFactory);
            weatherRepositoryConfiguration = new WeatherRepositoryConfiguration
            {
                ConnectionString = "Data Source=../../../testData.db"
            };

            loggerMock = new Mock<ILoggerAdapter<WeatherRepository>>();
        };
    }

    internal class When_retrieving_condition_for_years : WeatherRepositorySpecs
    {
        protected static IEnumerable<Condition> conditions;

        Establish context = () =>
            weatherRepository = new WeatherRepository(loggerMock.Object, weatherRepositoryDbProviderFactory, weatherRepositoryConfiguration);

        Because of = () =>
            conditions = weatherRepository.GetYear(Offset, Limit);

        It should_have_correct_number_of_records = () =>
            conditions.Count().ShouldEqual(3);

        It should_not_have_months = () =>
            conditions.Should().Contain(c => c.Month == null);

        It should_not_have_days = () =>
            conditions.Should().Contain(c => c.Day == null);

        It should_not_have_hours = () =>
            conditions.Should().Contain(c => c.Hour == null);
    }

    internal class When_retrieving_condition_for_years_fails : WeatherRepositorySpecs
    {
        protected static IEnumerable<Condition> conditions;

        Establish context = () =>
            weatherRepository = new WeatherRepository(loggerMock.Object, weatherRepositoryDbProviderFactory, weatherRepositoryConfiguration);

        Because of = () =>
            conditions = weatherRepository.GetYear(4, Limit);

        It should_have_correct_number_of_records = () =>
            conditions.ShouldBeEmpty();
    }

    internal class When_retrieving_condition_detail_for_an_hour : WeatherRepositorySpecs
    {
        protected static Condition condition;

        Establish context = () =>
            weatherRepository = new WeatherRepository(loggerMock.Object, weatherRepositoryDbProviderFactory, weatherRepositoryConfiguration);

        Because of = () =>
            condition = weatherRepository.GetHourDetail(2018, 9, 1, 1);

        It should_return_the_correct_data = () =>
            condition.Should().BeEquivalentTo(testCondition);
    }

    internal class When_retrieving_condition_detail_for_an_hour_fails : WeatherRepositorySpecs
    {
        protected static Condition condition;

        Establish context = () =>
            weatherRepository = new WeatherRepository(loggerMock.Object, weatherRepositoryDbProviderFactory, weatherRepositoryConfiguration);

        Because of = () =>
            condition = weatherRepository.GetHourDetail(1900, 9, 1, 1);

        It should_return_the_correct_data = () =>
            condition.ShouldBeNull();
    }

    internal class When_retrieving_year_count : WeatherRepositorySpecs
    {
        protected static int count;

        Establish context = () =>
            weatherRepository = new WeatherRepository(loggerMock.Object, weatherRepositoryDbProviderFactory, weatherRepositoryConfiguration);

        Because of = () =>
            count = weatherRepository.GetYearCount();

        It should_return_the_correct_count = () =>
            count.ShouldEqual(3);
    }
}
