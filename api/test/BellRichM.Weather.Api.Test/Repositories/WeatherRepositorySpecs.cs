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

        protected static Mock<ILoggerAdapter<WeatherRepository>> loggerMock;

        protected static WeatherRepository weatherRepository;
        protected static IEnumerable<Condition> conditions;

        public static WeatherRepositoryDbProviderFactory WeatherRepositoryDbProviderFactory { get; set; }

        public static WeatherRepositoryConfiguration WeatherRepositoryConfiguration { get; set; }
    }

    internal class When_doing_sonething : WeatherRepositorySpecs
    {
        Cleanup after = () =>
        {
        };

        Establish context = () =>
        {
            loggerMock = new Mock<ILoggerAdapter<WeatherRepository>>();
            weatherRepository = new WeatherRepository(loggerMock.Object, WeatherRepositoryDbProviderFactory, WeatherRepositoryConfiguration);
        };

        Because of = () =>
        {
            conditions = weatherRepository.GetYear(Offset, Limit);
        };

        It should_be_true = () =>
        {
            conditions.Should().BeEmpty();
        };
    }
}
