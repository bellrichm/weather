using BellRichM.Logging;
using BellRichM.Weather.Api.Configuration;
using BellRichM.Weather.Api.Data;
using BellRichM.Weather.Api.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Threading.Tasks;

namespace BellRichM.Weather.Api.Repositories
{
    /// <inheritdoc/>
    public class ObservationRepository : IObservationRepository
    {
        private readonly ILoggerAdapter<ObservationRepository> _logger;
        private readonly string _connectionString;
        private readonly DbProviderFactory _observationDbProviderFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="ObservationRepository"/> class.
        /// </summary>
        /// <param name="logger">The <see cref="ILoggerAdapter{T}"/>.</param>
        /// <param name="observationDbProviderFactory">The <see cref="ObservationRepositoryDbProviderFactory"/>.</param>
        /// <param name="observationRepositoryConfiguration">The config.</param>
        public ObservationRepository(ILoggerAdapter<ObservationRepository> logger, ObservationRepositoryDbProviderFactory observationDbProviderFactory, IObservationRepositoryConfiguration observationRepositoryConfiguration)
        {
            if (observationDbProviderFactory == null)
            {
                throw new ArgumentNullException(nameof(observationDbProviderFactory));
            }

            if (observationRepositoryConfiguration == null)
            {
                throw new ArgumentNullException(nameof(observationRepositoryConfiguration));
            }

            _logger = logger;
            _observationDbProviderFactory = observationDbProviderFactory.ObservationDbProviderFactory;
            _connectionString = observationRepositoryConfiguration.ConnectionString;
        }

        /// <inheritdoc/>
        public async Task<Observation> GetObservation(int dateTime)
        {
            _logger.LogDiagnosticDebug("GetObservation: {@dateTime}", dateTime);

            var statement = @"
SELECT
     year, month, day, hour, minute, dayOfYear, week,
     dateTime, usUnits, interval,
     barometer, pressure, altimeter, outTemp, outHumidity,
     windSpeed, windDir, windGust, windGustDir,
     rainRate, dewpoint, rain, windchill, heatindex,
     ET, radiation, UV,
     extraTemp1, extraTemp2, extraTemp3,
     soilTemp1, soilTemp2, soilTemp3, soilTemp4,
     leafTemp1, leafTemp2, extraHumid1, extraHumid2,
     soilMoist1, soilMoist2, soilMoist3, soilMoist4,
     leafWet1, leafWet2
FROM condition
WHERE
    -- month=@dateTime
    -- AND year>2015
    dateTime=@dateTime
";

            Observation observation = null;

            var dbConnection = _observationDbProviderFactory.CreateConnection();
            dbConnection.ConnectionString = _connectionString;
            using (dbConnection)
            {
                var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = statement;
                using (dbCommand)
                {
                    dbCommand.AddParamWithValue("@dateTime", dateTime);

                    dbConnection.Open();

                    using (var rdr = dbCommand.ExecuteReader())
                    {
                        if (await rdr.ReadAsync().ConfigureAwait(true))
                        {
                            observation = this.ReadObservation(rdr);
                        }
                    }
                }
            }

            return observation;
        }

        /// <inheritdoc/>
        public async Task<List<Observation>> GetObservations(TimePeriodModel timePeriodModel)
        {
            _logger.LogDiagnosticDebug("GetObservation: {@timePeriod}", timePeriodModel);
            if (timePeriodModel == null)
            {
                throw new ArgumentNullException(nameof(timePeriodModel));
            }

            var statement = @"
SELECT
     year, month, day, hour, minute, dayOfYear, week, 
     dateTime, usUnits, interval,
     barometer, pressure, altimeter, outTemp, outHumidity,
     windSpeed, windDir, windGust, windGustDir,
     rainRate, dewpoint, rain, windchill, heatindex,
     ET, radiation, UV,
     extraTemp1, extraTemp2, extraTemp3,
     soilTemp1, soilTemp2, soilTemp3, soilTemp4,
     leafTemp1, leafTemp2, extraHumid1, extraHumid2,
     soilMoist1, soilMoist2, soilMoist3, soilMoist4,
     leafWet1, leafWet2
FROM condition
WHERE
    -- month=@dateTime
    -- AND year>2015
    dateTime>=@startDateTime
    AND dateTime<=@endDateTime
";

            var observations = new List<Observation>();

            var dbConnection = _observationDbProviderFactory.CreateConnection();
            dbConnection.ConnectionString = _connectionString;
            using (dbConnection)
            {
                var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = statement;
                using (dbCommand)
                {
                    dbCommand.AddParamWithValue("@startDateTime", timePeriodModel.StartDateTime);
                    dbCommand.AddParamWithValue("@endDateTime", timePeriodModel.EndDateTime);

                    dbConnection.Open();

                    using (var rdr = dbCommand.ExecuteReader())
                    {
                        while (await rdr.ReadAsync().ConfigureAwait(true))
                        {
                            observations.Add(this.ReadObservation(rdr));
                        }
                    }
                }
            }

            return observations;
        }

        /// <inheritdoc/>
        public async Task<List<Timestamp>> GetTimestamps(TimePeriodModel timePeriodModel)
        {
            _logger.LogDiagnosticDebug("GetTimestamps: {@timePeriod}", timePeriodModel);
            if (timePeriodModel == null)
            {
                throw new ArgumentNullException(nameof(timePeriodModel));
            }

            var statement = @"
SELECT
     dateTime
FROM condition
WHERE
    -- month=@dateTime
    -- AND year>2015
    dateTime>=@startDateTime
    AND dateTime<=@endDateTime
";

            var timestamps = new List<Timestamp>();

            var dbConnection = _observationDbProviderFactory.CreateConnection();
            dbConnection.ConnectionString = _connectionString;
            using (dbConnection)
            {
                var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = statement;
                using (dbCommand)
                {
                    dbCommand.AddParamWithValue("@startDateTime", timePeriodModel.StartDateTime);
                    dbCommand.AddParamWithValue("@endDateTime", timePeriodModel.EndDateTime);

                    dbConnection.Open();

                    using (var rdr = dbCommand.ExecuteReader())
                    {
                        while (await rdr.ReadAsync().ConfigureAwait(true))
                        {
                            timestamps.Add(
                                new Timestamp
                                {
                                    DateTime = System.Convert.ToInt32(rdr["dateTime"], CultureInfo.InvariantCulture)
                                });
                        }
                    }
                }
            }

            return timestamps;
        }

        /// <inheritdoc/>
        public async Task<int> CreateObservations(List<Observation> observations)
        {
            if (observations == null)
            {
                throw new ArgumentNullException(nameof(observations));
            }

            int count = 0;
            var dbConnection = _observationDbProviderFactory.CreateConnection();
            dbConnection.ConnectionString = _connectionString;
            using (dbConnection)
            {
                dbConnection.Open();

                using (var transaction = dbConnection.BeginTransaction())
                {
                    foreach (var observation in observations)
                    {
                        count += await CreateObservation(dbConnection, observation).ConfigureAwait(true);
                    }

                    transaction.Commit();
                }

                dbConnection.Close();
            }

            return count;
        }

        /// <inheritdoc/>
        public async Task<int> CreateObservation(Observation observation)
        {
            _logger.LogDiagnosticDebug("CreateObservation: {@observation}", observation);
            if (observation == null)
            {
                throw new ArgumentNullException(nameof(observation));
            }

            var count = 0;
            var dbConnection = _observationDbProviderFactory.CreateConnection();
            dbConnection.ConnectionString = _connectionString;
            using (dbConnection)
            {
                dbConnection.Open();
                count = await CreateObservation(dbConnection, observation).ConfigureAwait(true);
                dbConnection.Close();
            }

            return count;
        }

        /// <inheritdoc/>
        public async Task<int> UpdateObservation(Observation observation)
        {
            _logger.LogDiagnosticDebug("UpdateObservation: {@observation}", observation);
            if (observation == null)
            {
                throw new ArgumentNullException(nameof(observation));
            }

            var statement = @"
UPDATE condition
    SET
        year = @year,
        month = @month,
        day  = @day,
        hour = @hour,
        minute = @minute,
        dayOfYear = @dayOfYear,
        week = @week,
        dateTime = @dateTime,
        usUnits = @usUnits,
        interval = @interval,
        barometer = @barometer,
        pressure = @pressure,
        altimeter = @altimeter,
        outTemp = @outTemp,
        outHumidity = @outHumidity,
        windSpeed = @windSpeed,
        windDir = @windDir,
        windGust = @windGust,
        windGustDir = @windGustDir,
        rainRate = @rainRate,
        rain = @rain,
        dewpoint = @dewpoint,
        windchill = @windchill,
        heatindex = @heatindex,
        ET = @ET,
        radiation = @radiation,
        UV = @UV,
        extraTemp1  = @extraTemp1,
        extraTemp2  = @extraTemp2,
        extraTemp3  = @extraTemp3,
        soilTemp1 = @soilTemp1,
        soilTemp2 = @soilTemp2,
        soilTemp3 = @soilTemp3,
        soilTemp4 = @soilTemp4,
        leafTemp1 = @leafTemp1,
        leafTemp2 = @leafTemp2,
        extraHumid1 = @extraHumid1,
        extraHumid2 = @extraHumid2,
        soilMoist1  = @soilMoist1,
        soilMoist2  = @soilMoist2,
        soilMoist3  = @soilMoist3,
        soilMoist4  = @soilMoist4,
        leafWet1 = @leafWet1,
        leafWet2 = @leafWet2
    WHERE
        dateTime=@dateTime
";

            var dbConnection = _observationDbProviderFactory.CreateConnection();
            dbConnection.ConnectionString = _connectionString;
            int count;
            using (dbConnection)
            {
                dbConnection.Open();
                count = await ExecuteNonQuery(dbConnection, statement, observation).ConfigureAwait(true);
                dbConnection.Close();
            }

            return count;
        }

        /// <inheritdoc/>
        public async Task<int> DeleteObservation(int dateTime)
        {
            _logger.LogDiagnosticDebug("DeleteObservation: {@dateTime}", dateTime);

            var statement = @"
DELETE FROM condition
    WHERE
        dateTime=@dateTime
";

            int rowCount;
            var dbConnection = _observationDbProviderFactory.CreateConnection();
            dbConnection.ConnectionString = _connectionString;
            using (dbConnection)
            {
                var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = statement;
                using (dbCommand)
                {
                    dbCommand.AddParamWithValue("@dateTime", dateTime);

                    dbConnection.Open();
                    rowCount = await dbCommand.ExecuteNonQueryAsync().ConfigureAwait(true);
                    dbCommand.Parameters.Clear();
                }

                dbConnection.Close();
            }

            return rowCount;
        }

        private static async Task<int> ExecuteNonQuery(DbConnection dbConnection, string statement, Observation observation)
        {
            int rowCount;

            var dbCommand = dbConnection.CreateCommand();
            #pragma warning disable CA2100 // Trusting that calling procedures are correct...
            dbCommand.CommandText = statement;
            #pragma warning disable CA2100
            using (dbCommand)
            {
                dbCommand.AddParamWithValue("@year", observation.Year);
                dbCommand.AddParamWithValue("@month", observation.Month);
                dbCommand.AddParamWithValue("@day", observation.Day);
                dbCommand.AddParamWithValue("@hour", observation.Hour);
                dbCommand.AddParamWithValue("@minute", observation.Minute);
                dbCommand.AddParamWithValue("@dayOfYear", observation.DayOfYear);
                dbCommand.AddParamWithValue("@week", observation.Week);
                dbCommand.AddParamWithValue("@dateTime", observation.DateTime);
                dbCommand.AddParamWithValue("@usUnits", observation.USUnits);
                dbCommand.AddParamWithValue("@interval", observation.Interval);
                dbCommand.AddParamWithValue("@barometer", observation.Barometer);
                dbCommand.AddParamWithValue("@pressure", observation.Pressure);
                dbCommand.AddParamWithValue("@altimeter", observation.Altimeter);
                dbCommand.AddParamWithValue("@outTemp", observation.OutsideTemperature);
                dbCommand.AddParamWithValue("@outHumidity", observation.OutsideHumidity);
                dbCommand.AddParamWithValue("@windSpeed", observation.WindSpeed);
                dbCommand.AddParamWithValue("@windDir", observation.WindDirection);
                dbCommand.AddParamWithValue("@windGust", observation.WindGust);
                dbCommand.AddParamWithValue("@windGustDir", observation.WindGustDirection);
                dbCommand.AddParamWithValue("@rainRate", observation.RainRate);
                dbCommand.AddParamWithValue("@rain", observation.Rain);
                dbCommand.AddParamWithValue("@dewpoint", observation.DewPoint);
                dbCommand.AddParamWithValue("@windchill", observation.Windchill);
                dbCommand.AddParamWithValue("@heatindex", observation.HeatIndex);
                dbCommand.AddParamWithValue("@ET", observation.Evapotranspiration);
                dbCommand.AddParamWithValue("@radiation", observation.Radiation);
                dbCommand.AddParamWithValue("@UV", observation.Ultraviolet);
                dbCommand.AddParamWithValue("@extraTemp1", observation.ExtraTemperature1);
                dbCommand.AddParamWithValue("@extraTemp2", observation.ExtraTemperature2);
                dbCommand.AddParamWithValue("@extraTemp3", observation.ExtraTemperature3);
                dbCommand.AddParamWithValue("@soilTemp1", observation.SoilTemperature1);
                dbCommand.AddParamWithValue("@soilTemp2", observation.SoilTemperature2);
                dbCommand.AddParamWithValue("@soilTemp3", observation.SoilTemperature3);
                dbCommand.AddParamWithValue("@soilTemp4", observation.SoilTemperature4);
                dbCommand.AddParamWithValue("@leafTemp1", observation.LeafTemperature1);
                dbCommand.AddParamWithValue("@leafTemp2", observation.LeafTemperature2);
                dbCommand.AddParamWithValue("@extraHumid1", observation.ExtraHumidity1);
                dbCommand.AddParamWithValue("@extraHumid2", observation.ExtraHumidity2);
                dbCommand.AddParamWithValue("@soilMoist1", observation.SoilMoisture1);
                dbCommand.AddParamWithValue("@soilMoist2", observation.SoilMoisture2);
                dbCommand.AddParamWithValue("@soilMoist3", observation.SoilMoisture3);
                dbCommand.AddParamWithValue("@soilMoist4", observation.SoilMoisture4);
                dbCommand.AddParamWithValue("@leafWet1", observation.LeafWetness1);
                dbCommand.AddParamWithValue("@leafWet2", observation.LeafWetness2);

                rowCount = await dbCommand.ExecuteNonQueryAsync().ConfigureAwait(true);
                dbCommand.Parameters.Clear();
            }

            return rowCount;
        }

        private async Task<int> CreateObservation(DbConnection dbConnection, Observation observation)
        {
            var statement = @"
INSERT INTO condition
    (
     year, month, day, hour, minute, dayOfYear, week,
     dateTime, usUnits, interval,
     barometer, pressure, altimeter, outTemp, outHumidity,
     windSpeed, windDir, windGust, windGustDir,
     rainRate, dewpoint, rain, windchill, heatindex,
     ET, radiation, UV,
     extraTemp1, extraTemp2, extraTemp3,
     soilTemp1, soilTemp2, soilTemp3, soilTemp4,
     leafTemp1, leafTemp2, extraHumid1, extraHumid2,
     soilMoist1, soilMoist2, soilMoist3, soilMoist4,
     leafWet1, leafWet2
    )
    VALUES
    (
     @year, @month, @day, @hour, @minute, @dayOfYear, @week,
     @dateTime, @usUnits, @interval,
     @barometer, @pressure, @altimeter, @outTemp, @outHumidity,
     @windSpeed, @windDir, @windGust, @windGustDir,
     @rainRate, @dewpoint, @rain, @windchill, @heatindex,
     @ET, @radiation, @UV,
     @extraTemp1, @extraTemp2, @extraTemp3,
     @soilTemp1, @soilTemp2, @soilTemp3, @soilTemp4,
     @leafTemp1, @leafTemp2, @extraHumid1, @extraHumid2,
     @soilMoist1, @soilMoist2, @soilMoist3, @soilMoist4,
     @leafWet1, @leafWet2
     )
";

            return await ExecuteNonQuery(dbConnection, statement, observation).ConfigureAwait(true);
        }

        private Observation ReadObservation(DbDataReader rdr)
        {
            return new Observation
            {
                Year = System.Convert.ToInt32(rdr["year"], CultureInfo.InvariantCulture),
                Month = System.Convert.ToInt32(rdr["month"], CultureInfo.InvariantCulture),
                Day = System.Convert.ToInt32(rdr["day"], CultureInfo.InvariantCulture),
                Hour = System.Convert.ToInt32(rdr["hour"], CultureInfo.InvariantCulture),
                Minute = System.Convert.ToInt32(rdr["minute"], CultureInfo.InvariantCulture),
                DayOfYear = System.Convert.ToInt32(rdr["dayOfYear"], CultureInfo.InvariantCulture),
                Week = System.Convert.ToInt32(rdr["week"], CultureInfo.InvariantCulture),
                DateTime = System.Convert.ToInt32(rdr["dateTime"], CultureInfo.InvariantCulture),
                USUnits = System.Convert.ToInt32(rdr["usUnits"], CultureInfo.InvariantCulture),
                Interval = System.Convert.ToInt32(rdr["interval"], CultureInfo.InvariantCulture),
                Barometer = rdr.GetValue<double>("barometer"),
                Pressure = rdr.GetValue<double>("pressure"),
                Altimeter = rdr.GetValue<double>("altimeter"),
                OutsideTemperature = rdr.GetValue<double>("outTemp"),
                OutsideHumidity = rdr.GetValue<double>("outHumidity"),
                WindSpeed = rdr.GetValue<double>("windSpeed"),
                WindDirection = rdr.GetValue<double>("windDir"),
                WindGust = rdr.GetValue<double>("windGust"),
                WindGustDirection = rdr.GetValue<double>("windGustDir"),
                RainRate = rdr.GetValue<double>("rainRate"),
                Rain = rdr.GetValue<double>("rain"),
                DewPoint = rdr.GetValue<double>("dewpoint"),
                Windchill = rdr.GetValue<double>("windchill"),
                HeatIndex = rdr.GetValue<double>("heatindex"),
                Evapotranspiration = rdr.GetValue<double>("ET"),
                Radiation = rdr.GetValue<double>("radiation"),
                Ultraviolet = rdr.GetValue<double>("UV"),
                ExtraTemperature1 = rdr.GetValue<double>("extraTemp1"),
                ExtraTemperature2 = rdr.GetValue<double>("extraTemp2"),
                ExtraTemperature3 = rdr.GetValue<double>("extraTemp3"),
                SoilTemperature1 = rdr.GetValue<double>("soilTemp1"),
                SoilTemperature2 = rdr.GetValue<double>("soilTemp2"),
                SoilTemperature3 = rdr.GetValue<double>("soilTemp3"),
                SoilTemperature4 = rdr.GetValue<double>("soilTemp4"),
                LeafTemperature1 = rdr.GetValue<double>("leafTemp1"),
                LeafTemperature2 = rdr.GetValue<double>("leafTemp2"),
                ExtraHumidity1 = rdr.GetValue<double>("extraHumid1"),
                ExtraHumidity2 = rdr.GetValue<double>("extraHumid2"),
                SoilMoisture1 = rdr.GetValue<double>("soilMoist1"),
                SoilMoisture2 = rdr.GetValue<double>("soilMoist2"),
                SoilMoisture3 = rdr.GetValue<double>("soilMoist3"),
                SoilMoisture4 = rdr.GetValue<double>("soilMoist4"),
                LeafWetness1 = rdr.GetValue<double>("leafWet1"),
                LeafWetness2 = rdr.GetValue<double>("leafWet2")
            };
        }
    }
}
