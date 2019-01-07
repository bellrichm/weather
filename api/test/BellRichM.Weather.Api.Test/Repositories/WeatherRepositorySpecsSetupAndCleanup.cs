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
    public class WeatherRepositorySpecsSetupAndCleanup : IAssemblyContext
    {
        private DbConnection masterConnection;

        public void OnAssemblyStart()
        {
            var createTableSQL = @"
CREATE TABLE v_condition (
       year   INTEGER NOT NULL,
       month   INTEGER NOT NULL,
       day   INTEGER NOT NULL,
       hour  INTEGER NOT NULL,
       minute INTEGER NOT NULL,
       dateTime   INTEGER NOT NULL,
       usUnits   INTEGER NOT NULL,
       interval   INTEGER NOT NULL,
       barometer   REAL,
       pressure   REAL,
       altimeter   REAL,
       outTemp   REAL,
       outHumidity   REAL,
       windSpeed   REAL,
       windDir   REAL,
       windGust   REAL,
       windGustDir   REAL,
       rainRate   REAL,
       rain   REAL,
       dewpoint   REAL,
       windchill   REAL,
       heatindex   REAL,
       ET   REAL,
       radiation   REAL,
       UV   REAL,
       extraTemp1   REAL,
       extraTemp2   REAL,
       extraTemp3   REAL,
       soilTemp1   REAL,
       soilTemp2   REAL,
       soilTemp3   REAL,
       soilTemp4   REAL,
       leafTemp1   REAL,
       leafTemp2   REAL,
       extraHumid1   REAL,
       extraHumid2   REAL,
       soilMoist1   REAL,
       soilMoist2   REAL,
       soilMoist3   REAL,
       soilMoist4   REAL,
       leafWet1   REAL,
       leafWet2   REAL,
     PRIMARY KEY (year, month, day, hour, minute)
     );
            ";
            var dbProviderFactory = SqliteFactory.Instance;
            WeatherRepositorySpecs.WeatherRepositoryDbProviderFactory = new WeatherRepositoryDbProviderFactory(dbProviderFactory);
            WeatherRepositorySpecs.WeatherRepositoryConfiguration = new WeatherRepositoryConfiguration
            {
                ConnectionString = "Data Source=InMemorySample;Mode=Memory;Cache=Shared"
            };

            // The in-memory database only persists while a connection is open to it. To manage
            // its lifetime, keep one open connection arround for as long as you need it.
            masterConnection = WeatherRepositorySpecs.WeatherRepositoryDbProviderFactory.WeatherDbProviderFactory.CreateConnection();
            masterConnection.ConnectionString = WeatherRepositorySpecs.WeatherRepositoryConfiguration.ConnectionString;
            masterConnection.Open();

            RunSQL(createTableSQL);
        }

        public void OnAssemblyComplete()
        {
            masterConnection.Close();
        }

        private static void RunSQL(string command)
        {
            var dbConnection = WeatherRepositorySpecs.WeatherRepositoryDbProviderFactory.WeatherDbProviderFactory.CreateConnection();
            dbConnection.ConnectionString = WeatherRepositorySpecs.WeatherRepositoryConfiguration.ConnectionString;

            using (dbConnection)
            {
                var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = command;
                using (dbCommand)
                {
                    dbConnection.Open();
                    dbCommand.ExecuteNonQuery();
                }
            }
        }
    }
}
