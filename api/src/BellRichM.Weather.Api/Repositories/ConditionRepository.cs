using BellRichM.Logging;
using BellRichM.Weather.Api.Configuration;
using BellRichM.Weather.Api.Data;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Threading.Tasks;

namespace BellRichM.Weather.Api.Repositories
{
    /// <inheritdoc/>
    public class ConditionRepository : IConditionRepository
    {
        private const string DataFields = @"
  , CAST(MAX(outTemp) as TEXT) as maxTemp
  , CAST(MIN(outTemp) as TEXT) as minTemp
  , CAST(MAX(outHumidity) as TEXT) as maxHumidity
  , CAST(MIN(outHumidity) as TEXT) as minHumidity
  , CAST(MAX(dewpoint) as TEXT) as maxDewpoint
  , CAST(MIN(dewpoint) as TEXT) as minDewpoint
  , CAST(MAX(heatIndex) as TEXT) as maxHeatIndex
  , CAST(MIN(windchill) as TEXT) as minWindchill
  , CAST(MAX(barometer) as TEXT) as maxBarometer
  , CAST(MIN(barometer) as TEXT) as minBarometer
  , CAST(MAX(ET) as TEXT) as maxET
  , CAST(MIN(ET) as TEXT) as minET
  , CAST(MAX(UV) as TEXT) as maxUV
  , CAST(MIN(UV) as TEXT) as minUV
  , CAST(MAX(radiation) as TEXT) as maxRadiation
  , CAST(MIN(radiation) as TEXT) as minRadiation
  , CAST(MAX(rainRate) as TEXT) as maxRainRate
  , CAST(SUM(rain) as TEXT) as rainTotal
  , CAST(MAX(windGust) as TEXT) as maxWindGust
  , CAST(AVG(windSpeed) as TEXT) as avgWindSpeed

FROM condition
        ";

        private readonly ILoggerAdapter<ConditionRepository> _logger;
        private readonly string _connectionString;
        private readonly DbProviderFactory _conditionDbProviderFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConditionRepository"/> class.
        /// </summary>
        /// <param name="logger">The <see cref="ILoggerAdapter{T}"/>.</param>
        /// <param name="conditionDbProviderFactory">The <see cref="ConditionRepositoryDbProviderFactory"/>.</param>
        /// <param name="conditionRepositoryConfiguration">The config.</param>
        public ConditionRepository(ILoggerAdapter<ConditionRepository> logger, ConditionRepositoryDbProviderFactory conditionDbProviderFactory, IConditionRepositoryConfiguration conditionRepositoryConfiguration)
        {
            if (conditionRepositoryConfiguration == null)
            {
                throw new ArgumentNullException(nameof(conditionRepositoryConfiguration));
            }

            if (conditionDbProviderFactory == null)
            {
                throw new ArgumentNullException(nameof(conditionDbProviderFactory));
            }

            _logger = logger;
            _conditionDbProviderFactory = conditionDbProviderFactory.ConditionDbProviderFactory;
            _connectionString = conditionRepositoryConfiguration.ConnectionString;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MinMaxCondition>> GetYear(int offset, int limit)
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

            var records = new List<MinMaxCondition>();

            var dbConnection = _conditionDbProviderFactory.CreateConnection();
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
                        while (await rdr.ReadAsync().ConfigureAwait(true))
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
        public async Task<MinMaxCondition> GetHourDetail(int year, int month, int day, int hour)
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

            MinMaxCondition minMaxCondition = null;

            var dbConnection = _conditionDbProviderFactory.CreateConnection();
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
                        while (await rdr.ReadAsync().ConfigureAwait(true))
                        {
                            minMaxCondition = ReadDataFields(rdr);
                            minMaxCondition.Year = System.Convert.ToInt32(rdr["year"], CultureInfo.InvariantCulture);
                            minMaxCondition.Month = System.Convert.ToInt32(rdr["month"], CultureInfo.InvariantCulture);
                            minMaxCondition.Day = System.Convert.ToInt32(rdr["day"], CultureInfo.InvariantCulture);
                            minMaxCondition.Hour = System.Convert.ToInt32(rdr["hour"], CultureInfo.InvariantCulture);
                        }
                    }
                }
            }

            return minMaxCondition;
        }

        /// <inheritdoc/>
        public async Task<int> GetYearCount()
        {
            _logger.LogDiagnosticDebug("GetYearCount");

            var statement = @"
SELECT COUNT(DISTINCT year) as yearCount
  FROM v_condition
;
            ";

            int yearCount = 0;

            var dbConnection = _conditionDbProviderFactory.CreateConnection();
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
                        if (await rdr.ReadAsync().ConfigureAwait(true))
                        {
                            yearCount = System.Convert.ToInt32(rdr["yearCount"], CultureInfo.InvariantCulture);
                        }
                    }
                }

                return yearCount;
            }
        }

        private MinMaxCondition ReadDataFields(DbDataReader rdr)
        {
            var minMaxCondition = new MinMaxCondition
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
            return minMaxCondition;
        }
    }
}