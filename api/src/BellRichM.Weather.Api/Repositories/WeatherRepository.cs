using BellRichM.Logging;
using BellRichM.Weather.Api.Configuration;
using BellRichM.Weather.Api.Data;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;

namespace BellRichM.Weather.Api.Repositories
{
    /// <inheritdoc/>
    public class WeatherRepository : IWeatherRepository
    {
        private const string DataFields = @"
  , MAX(outTemp) as maxTemp
  , MIN(outTemp) as minTemp
  , MAX(outHumidity) as maxHumidity
  , MIN(outHumidity) as minHumidity
  , MAX(dewpoint) as maxDewpoint
  , MIN(dewpoint) as minDewpoint
  , MAX(heatIndex) as maxHeatIndex
  , MIN(windchill) as minWindchill
  , MAX(barometer) as maxBarometer
  , MIN(barometer) as minBarometer
  , MAX(ET) as maxET
  , MIN(ET) as minET
  , MAX(UV) as maxUV
  , MIN(UV) as minUV
  , MAX(radiation) as maxRadiation
  , MIN(radiation) as minRadiation
  , MAX(rainRate) as maxRainRate
  , SUM(rain) as rainTotal
  , MAX(windGust) as maxWindGust
  , AVG(windSpeed) as avgWindSpeed
FROM v_condition
        ";

        private readonly ILoggerAdapter<WeatherRepository> _logger;
        private readonly string _connectionString;
        private readonly DbProviderFactory _weatherDbProviderFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeatherRepository"/> class.
        /// </summary>
        /// <param name="logger">The <see cref="ILoggerAdapter{T}"/>.</param>
        /// <param name="weatherDbProviderFactory">The <see cref="WeatherRepositoryDbProviderFactory"/>.</param>
        /// <param name="weatherRepositoryConfiguration">The config.</param>
        public WeatherRepository(ILoggerAdapter<WeatherRepository> logger, WeatherRepositoryDbProviderFactory weatherDbProviderFactory, IWeatherRepositoryConfiguration weatherRepositoryConfiguration)
        {
            _logger = logger;
            _weatherDbProviderFactory = weatherDbProviderFactory.WeatherDbProviderFactory;
            _connectionString = weatherRepositoryConfiguration.ConnectionString;
        }

        /// <inheritdoc/>
        public IEnumerable<Condition> GetYear(int offset, int limit)
        {
            _logger.LogDiagnosticDebug("GetYear: {@offset} {@limit}", offset, limit);

            var statement = "SELECT year"
            + DataFields
            + @"
GROUP BY year
ORDER BY year
LIMIT @limit
OFFSET @offset
;
            ";

            var records = new List<Condition>();

            var dbConnection = _weatherDbProviderFactory.CreateConnection();
            dbConnection.ConnectionString = _connectionString;
            using (dbConnection)
            {
                var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = statement;
                using (dbCommand)
                {
                    dbCommand.AddParamWithValue("@offset", offset);
                    dbCommand.AddParamWithValue("@limit", limit);

                    dbConnection.Open();

                    using (var rdr = dbCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            var condition = ReadDataFields(rdr);
                            condition.Year = System.Convert.ToInt32(rdr["year"], CultureInfo.InvariantCulture);
                            records.Add(condition);
                        }
                    }
                }
            }

            return records;
        }

        /// <inheritdoc/>
        public Condition GetHourDetail(int year, int month, int day, int hour)
        {
            _logger.LogDiagnosticDebug("GetHourDetail: {@year} {@month} {@day} {@hour}", year, month, day, hour);

            var statement = @"
SELECT
  year, month, day, hour"
            +
            DataFields
            + @"
WHERE
    year = @year
AND
    month = @month
AND
    DAY = @day
AND
    HOUR = @hour
GROUP BY year, month, day, hour
;
            ";

            Condition condition = null;

            var dbConnection = _weatherDbProviderFactory.CreateConnection();
            dbConnection.ConnectionString = _connectionString;
            using (dbConnection)
            {
                var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = statement;
                using (dbCommand)
                {
                    dbCommand.AddParamWithValue("@year", year);
                    dbCommand.AddParamWithValue("@month", month);
                    dbCommand.AddParamWithValue("@day", day);
                    dbCommand.AddParamWithValue("@hour", hour);

                    dbConnection.Open();

                    using (var rdr = dbCommand.ExecuteReader())
                    {
                        while (rdr.Read())
                        {
                            System.Console.WriteLine("reading");
                            condition = ReadDataFields(rdr);
                            condition.Year = System.Convert.ToInt32(rdr["year"], CultureInfo.InvariantCulture);
                            condition.Month = System.Convert.ToInt32(rdr["month"], CultureInfo.InvariantCulture);
                            condition.Day = System.Convert.ToInt32(rdr["day"], CultureInfo.InvariantCulture);
                            condition.Hour = System.Convert.ToInt32(rdr["hour"], CultureInfo.InvariantCulture);
                        }
                    }
                }
            }

            return condition;
        }

        /// <inheritdoc/>
        public int GetYearCount()
        {
            _logger.LogDiagnosticDebug("GetYearCount");

            var statement = @"
SELECT COUNT(DISTINCT year) as yearCount
  FROM v_condition
;
            ";

            int yearCount = 0;

            var dbConnection = _weatherDbProviderFactory.CreateConnection();
            dbConnection.ConnectionString = _connectionString;
            using (dbConnection)
            {
                var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = statement;
                using (dbCommand)
                {
                    dbConnection.Open();

                    using (var rdr = dbCommand.ExecuteReader())
                    {
                        if (rdr.Read())
                        {
                            yearCount = System.Convert.ToInt32(rdr["yearCount"], CultureInfo.InvariantCulture);
                        }
                    }
                }

                return yearCount;
            }
        }

        private Condition ReadDataFields(DbDataReader rdr)
        {
            var condition = new Condition
            {
                MaxTemp = rdr.GetStringValue("maxTemp"),
                MinTemp = rdr.GetStringValue("minTemp"),
                MaxHumidity = rdr.GetStringValue("maxHumidity"),
                MinHumidity = rdr.GetStringValue("minHumidity"),
                MaxDewpoint = rdr.GetStringValue("maxDewpoint"),
                MinDewpoint = rdr.GetStringValue("minDewpoint"),
                MaxHeatIndex = rdr.GetStringValue("maxHeatIndex"),
                MinWindchill = rdr.GetStringValue("minWindchill"),
                MaxBarometer = rdr.GetStringValue("maxBarometer"),
                MinBarometer = rdr.GetStringValue("minBarometer"),
                MaxET = rdr.GetStringValue("maxET"),
                MinET = rdr.GetStringValue("minET"),
                MaxUV = rdr.GetStringValue("maxUV"),
                MinUV = rdr.GetStringValue("minUV"),
                MaxRadiation = rdr.GetStringValue("maxRadiation"),
                MinRadiation = rdr.GetStringValue("minRadiation"),
                MaxRainRate = rdr.GetStringValue("maxRainRate"),
                RainTotal = rdr.GetStringValue("rainTotal"),
                MaxWindGust = rdr.GetStringValue("maxWindGust"),
                AvgWindSpeed = rdr.GetStringValue("avgWindSpeed"),
            };
            return condition;
        }
    }
}