using BellRichM.Logging;
using BellRichM.Weather.Api.Configuration;
using BellRichM.Weather.Api.Data;
using BellRichM.Weather.Api.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
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

        private const string GroupSelect = @"
SELECT 
	c1.year, c1.month, c1.day, 
	c1.windGustDir, c1.windGust, AVG(c1.windDir) as windDir, AVG(c1.windSpeed) as windSpeed, 
	AVG(c1.outTemp) as outTemp, AVG(c1.heatindex) as heatindex, AVG(windchill) as windchill, AVG(dewpoint) as dewpoint, AVG(barometer) as barometer,
    SUM(c1.rain) as rain, AVG(c1.rainRate) as rainRate, AVG(c1.outHumidity) as outHumidity
FROM condition c1 
	INNER JOIN ( 
		SELECT 
			MAX(windGust) as windGustMax, year, month, day 
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
                            var minMaxCondition = ReadDataFields(rdr);
                            minMaxCondition.Year = System.Convert.ToInt32(rdr["year"], CultureInfo.InvariantCulture);
                            records.Add(minMaxCondition);
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
        public async Task<IEnumerable<MinMaxGroup>> GetMinMaxConditionsByMinute(int dayOfYear, int startHour, int endHour, int offset, int limit)
        {
            _logger.LogDiagnosticDebug("GetYear: {@offset} {@limit}", offset, limit);

            var statement = "SELECT year, dayOfYear, hour, minute"
            + DataFields
            + @"
WHERE
    dayofYear = @dayOfYear
    AND hour <= @endHour
    AND hour >= @startHour 
GROUP BY year, dayOfYear, hour, minute
ORDER BY dayOfYear, hour, minute
LIMIT @limit
OFFSET @offset
;
            ";
            _logger.LogDiagnosticDebug("statement:\n {statement}\n", statement);
            var minMaxGroups = new List<MinMaxGroup>();

            var dbConnection = _conditionDbProviderFactory.CreateConnection();
            dbConnection.ConnectionString = _connectionString;
            using (dbConnection)
            {
                var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = statement;
                using (dbCommand)
                {
                    dbCommand.AddParamWithValue("@dayOfYear", dayOfYear);
                    dbCommand.AddParamWithValue("@startHour", startHour);
                    dbCommand.AddParamWithValue("@endHour", endHour);
                    dbCommand.AddParamWithValue("@offset", offset);
                    dbCommand.AddParamWithValue("@limit", 1000); // TODO temp to dump out some test data

                    dbConnection.Open();

                    using (var rdr = dbCommand.ExecuteReader())
                    {
                        while (await rdr.ReadAsync().ConfigureAwait(true))
                        {
                            var minMaxCondition = ReadDataFields(rdr);
                            minMaxCondition.Year = System.Convert.ToInt32(rdr["year"], CultureInfo.InvariantCulture);
                            minMaxCondition.DayOfYear = System.Convert.ToInt32(rdr["dayofYear"], CultureInfo.InvariantCulture);
                            minMaxCondition.Hour = System.Convert.ToInt32(rdr["hour"], CultureInfo.InvariantCulture);
                            minMaxCondition.Minute = System.Convert.ToInt32(rdr["minute"], CultureInfo.InvariantCulture);
                            var minMaxGroup = minMaxGroups.FirstOrDefault(g =>
                                                                          g.DayOfYear == minMaxCondition.DayOfYear &&
                                                                          g.Hour == minMaxCondition.Hour &&
                                                                          g.Minute == minMaxCondition.Minute);

                            if (minMaxGroup is null)
                            {
                                minMaxGroup = new MinMaxGroup
                                {
                                    DayOfYear = minMaxCondition.DayOfYear,
                                    Hour = minMaxCondition.Hour,
                                    Minute = minMaxCondition.Minute
                                };
                                minMaxGroups.Add(minMaxGroup);
                            }

                            minMaxGroup.MinMaxConditions.Add(minMaxCondition);
                        }
                    }
                }
            }

            return minMaxGroups;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MinMaxGroup>> GetMinMaxConditionsByHour(int startDayOfYear, int endDayOfYear, int offset, int limit)
        {
            _logger.LogDiagnosticDebug("GetYear: {@offset} {@limit}", offset, limit);

            var statement = "SELECT year, month, day, dayOfYear, hour"
            + DataFields
            + @"
WHERE
    dayofYear <= @endDayOfYear
    AND dayofYear >= @startDayOfYear 
GROUP BY year, dayOfYear, hour 
ORDER BY dayOfYear, hour
LIMIT @limit
OFFSET @offset
;
            ";
            _logger.LogDiagnosticDebug("statement:\n {statement}\n", statement);
            var minMaxGroups = new List<MinMaxGroup>();

            var dbConnection = _conditionDbProviderFactory.CreateConnection();
            dbConnection.ConnectionString = _connectionString;
            using (dbConnection)
            {
                var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = statement;
                using (dbCommand)
                {
                    dbCommand.AddParamWithValue("@startDayOfYear", startDayOfYear);
                    dbCommand.AddParamWithValue("@endDayOfYear", endDayOfYear);
                    dbCommand.AddParamWithValue("@offset", offset);
                    dbCommand.AddParamWithValue("@limit", 100000); // TODO temp to dump out some test data

                    dbConnection.Open();

                    using (var rdr = dbCommand.ExecuteReader())
                    {
                        while (await rdr.ReadAsync().ConfigureAwait(true))
                        {
                            var minMaxCondition = ReadDataFields(rdr);
                            minMaxCondition.Year = System.Convert.ToInt32(rdr["year"], CultureInfo.InvariantCulture);
                            minMaxCondition.Month = System.Convert.ToInt32(rdr["month"], CultureInfo.InvariantCulture);
                            minMaxCondition.Day = System.Convert.ToInt32(rdr["day"], CultureInfo.InvariantCulture);
                            minMaxCondition.DayOfYear = System.Convert.ToInt32(rdr["dayofYear"], CultureInfo.InvariantCulture);
                            minMaxCondition.Hour = System.Convert.ToInt32(rdr["hour"], CultureInfo.InvariantCulture);
                            var minMaxGroup = minMaxGroups.FirstOrDefault(g =>
                                                                          g.DayOfYear == minMaxCondition.DayOfYear &&
                                                                          g.Hour == minMaxCondition.Hour);

                            if (minMaxGroup is null)
                            {
                                minMaxGroup = new MinMaxGroup
                                {
                                    DayOfYear = minMaxCondition.DayOfYear,
                                    Hour = minMaxCondition.Hour
                                };
                                minMaxGroups.Add(minMaxGroup);
                            }

                            minMaxGroup.MinMaxConditions.Add(minMaxCondition);
                        }
                    }
                }
            }

            return minMaxGroups;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MinMaxGroup>> GetMinMaxConditionsByDay(int startDayOfYear, int endDayOfYear, int offset, int limit)
        {
            _logger.LogDiagnosticDebug("GetYear: {@offset} {@limit}", offset, limit);

            var statement = "SELECT year, month, day"
            + " , CAST(strftime('%j', '2016-' || substr('00' || month, -2, 2) || '-' || substr('00' || day, -2, 2)) as INT) as dayOfYear"
            + DataFields
            + @"
WHERE
    CAST(strftime('%j', '2016-' || substr('00' || month, -2, 2) || '-' || substr('00' || day, -2, 2)) as INT) <= @endDayOfYear 
    AND 
    CAST(strftime('%j', '2016-' || substr('00' || month, -2, 2) || '-' || substr('00' || day, -2, 2)) as INT) >= @startDayOfYear 
GROUP BY year, month, day
ORDER BY month, day, year
LIMIT @limit
OFFSET @offset
;
            ";
            _logger.LogDiagnosticDebug("statement:\n {statement}\n", statement);
            var minMaxGroups = new List<MinMaxGroup>();

            var dbConnection = _conditionDbProviderFactory.CreateConnection();
            dbConnection.ConnectionString = _connectionString;
            using (dbConnection)
            {
                var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = statement;
                using (dbCommand)
                {
                    dbCommand.AddParamWithValue("@startDayOfYear", startDayOfYear);
                    dbCommand.AddParamWithValue("@endDayOfYear", endDayOfYear);
                    dbCommand.AddParamWithValue("@offset", offset);
                    dbCommand.AddParamWithValue("@limit", 10000); // TODO temp to dump out some test data

                    dbConnection.Open();

                    using (var rdr = dbCommand.ExecuteReader())
                    {
                        while (await rdr.ReadAsync().ConfigureAwait(true))
                        {
                            var minMaxCondition = ReadDataFields(rdr);
                            minMaxCondition.Year = System.Convert.ToInt32(rdr["year"], CultureInfo.InvariantCulture);
                            minMaxCondition.Month = System.Convert.ToInt32(rdr["month"], CultureInfo.InvariantCulture);
                            minMaxCondition.Day = System.Convert.ToInt32(rdr["day"], CultureInfo.InvariantCulture);
                            minMaxCondition.DayOfYear = System.Convert.ToInt32(rdr["dayOfYear"], CultureInfo.InvariantCulture);
                            var minMaxGroup = minMaxGroups.FirstOrDefault(g =>
                                                                          g.Month == minMaxCondition.Month &&
                                                                          g.Day == minMaxCondition.Day);

                            if (minMaxGroup is null)
                            {
                                minMaxGroup = new MinMaxGroup
                                {
                                    Month = minMaxCondition.Month,
                                    Day = minMaxCondition.Day,
                                    DayOfYear = minMaxCondition.DayOfYear
                                };
                                minMaxGroups.Add(minMaxGroup);
                            }

                            minMaxGroup.MinMaxConditions.Add(minMaxCondition);
                        }
                    }
                }
            }

            return minMaxGroups;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<MinMaxGroup>> GetMinMaxConditionsByWeek(int startWeekOfYear, int endWeekOfYear, int offset, int limit)
        {
            _logger.LogDiagnosticDebug("GetYear: {@offset} {@limit}", offset, limit);

            var statement = "SELECT year, week"
            + DataFields
            + @"
WHERE
    week <= @endWeekOfYear 
    AND 
    week >= @startWeekOfYear
GROUP BY year, week
ORDER BY week, year
LIMIT @limit
OFFSET @offset
;
            ";
            _logger.LogDiagnosticDebug("statement:\n {statement}\n", statement);
            var minMaxGroups = new List<MinMaxGroup>();

            var dbConnection = _conditionDbProviderFactory.CreateConnection();
            dbConnection.ConnectionString = _connectionString;
            using (dbConnection)
            {
                var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = statement;
                using (dbCommand)
                {
                    dbCommand.AddParamWithValue("@startWeekOfYear", startWeekOfYear);
                    dbCommand.AddParamWithValue("@endWeekOfYear", endWeekOfYear);
                    dbCommand.AddParamWithValue("@offset", offset);
                    dbCommand.AddParamWithValue("@limit", 10000); // TODO temp to dump out some test data

                    dbConnection.Open();

                    using (var rdr = dbCommand.ExecuteReader())
                    {
                        while (await rdr.ReadAsync().ConfigureAwait(true))
                        {
                            var minMaxCondition = ReadDataFields(rdr);
                            minMaxCondition.Year = System.Convert.ToInt32(rdr["year"], CultureInfo.InvariantCulture);
                            minMaxCondition.Week = System.Convert.ToInt32(rdr["week"], CultureInfo.InvariantCulture);
                            var minMaxGroup = minMaxGroups.FirstOrDefault(g =>
                                                                          g.Week == minMaxCondition.Week);

                            if (minMaxGroup is null)
                            {
                                minMaxGroup = new MinMaxGroup
                                {
                                    Week = minMaxCondition.Week
                                };
                                minMaxGroups.Add(minMaxGroup);
                            }

                            minMaxGroup.MinMaxConditions.Add(minMaxCondition);
                        }
                    }
                }
            }

            return minMaxGroups;
        }

        /// <inheritdoc/>
        public async Task<IEnumerable<Condition>> GetConditionsByDay(int offset, int limit, TimePeriodModel timePeriodModel)
        {
            _logger.LogDiagnosticDebug("GetConditionsByDay: {@offset} {@limit} {@timePeriod}", offset, limit, timePeriodModel);
            if (timePeriodModel == null)
            {
                throw new ArgumentNullException(nameof(timePeriodModel));
            }

            var statement = GroupSelect + @"
		GROUP BY year, month, day 
		) as c2 
		ON c1.windGust = c2.windGustMax 
            AND  c1.year = c2.year AND c1.month = c2.month AND c1.day = c2.day 
WHERE
    dateTime>=@startDateTime
    AND dateTime<=@endDateTime
GROUP BY c1.year, c1.month, c1.day
LIMIT @limit
OFFSET @offset
;";
            _logger.LogDiagnosticDebug("statement:\n {statement}\n", statement);

            var conditions = new List<Condition>();

            var dbConnection = _conditionDbProviderFactory.CreateConnection();
            dbConnection.ConnectionString = _connectionString;
            using (dbConnection)
            {
                var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = statement;
                using (dbCommand)
                {
                    dbCommand.AddParamWithValue("@startDateTime", timePeriodModel.StartDateTime);
                    dbCommand.AddParamWithValue("@endDateTime", timePeriodModel.EndDateTime);
                    dbCommand.AddParamWithValue("@offset", offset);
                    dbCommand.AddParamWithValue("@limit", 10000); // TODO temp to dump out some test data

                    dbConnection.Open();

                    using (var rdr = dbCommand.ExecuteReader())
                    {
                        while (await rdr.ReadAsync().ConfigureAwait(true))
                        {
                            conditions.Add(this.ReadGroupedCondition(rdr));
                        }
                    }
                }
            }

            return conditions;
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

        /// <inheritdoc/>
        public async Task<int> GetDayCount()
        {
            _logger.LogDiagnosticDebug("GetDayCount");

            // TODO - Find a faster way
            var statement = @"
SELECT COUNT(DISTINCT CAST(year as TEXT) || CAST(month as TEXT) || CAST(day as TEXT)) as dataCount
  FROM condition 
;
            ";

            return await GetDataCount(statement).ConfigureAwait(true);
        }

        private async Task<int> GetDataCount(string statement)
        {
            int dataCount = 0;

            var dbConnection = _conditionDbProviderFactory.CreateConnection();
            dbConnection.ConnectionString = _connectionString;
            using (dbConnection)
            {
                var dbCommand = dbConnection.CreateCommand();
                #pragma warning disable CA2100 // Trusting that calling procedures are correct...
                dbCommand.CommandText = statement;
                #pragma warning restore CA2100
                using (dbCommand)
                {
                    dbConnection.Open();

                    using (var rdr = dbCommand.ExecuteReader())
                    {
                        if (await rdr.ReadAsync().ConfigureAwait(true))
                        {
                            dataCount = System.Convert.ToInt32(rdr["dataCount"], CultureInfo.InvariantCulture);
                        }
                    }
                }

                return dataCount;
            }
        }

        private Condition ReadGroupedCondition(DbDataReader rdr)
        {
            return new Condition
            {
                Year = System.Convert.ToInt32(rdr["year"], CultureInfo.InvariantCulture),
                Month = System.Convert.ToInt32(rdr["month"], CultureInfo.InvariantCulture),
                Day = System.Convert.ToInt32(rdr["day"], CultureInfo.InvariantCulture),
                WindGustDirection = rdr.GetValue<double>("windGustDir"),
                WindGust = rdr.GetValue<double>("windGust"),
                WindDirection = rdr.GetValue<double>("windDir"),
                WindSpeed = rdr.GetValue<double>("windSpeed"),
                OutsideTemperature = rdr.GetValue<double>("outTemp"),
                HeatIndex = rdr.GetValue<double>("heatindex"),
                Windchill = rdr.GetValue<double>("windchill"),
                Barometer = rdr.GetValue<double>("barometer"),
                DewPoint = rdr.GetValue<double>("dewpoint"),
                Rain = rdr.GetValue<double>("rain"),
                RainRate = rdr.GetValue<double>("rainRate"),
                OutsideHumidity = rdr.GetValue<double>("outHumidity"),
            };
        }

        private MinMaxCondition ReadDataFields(DbDataReader rdr)
        {
            var minMaxcondition = new MinMaxCondition
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
            return minMaxcondition;
        }
    }
}
