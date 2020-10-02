using BellRichM.Logging;
using BellRichM.Weather.Api.Models;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Threading.Tasks;

namespace InitObservations
{
    /// <inheritdoc/>
    public class WeeWXRepository : IWeeWXRepository
    {
        private readonly ILoggerAdapter<WeeWXRepository> _logger;
        private readonly string _connectionString;
        private readonly DbProviderFactory _weeWXDbProviderFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeeWXRepository"/> class.
        /// </summary>
        /// <param name="logger">The <see cref="ILoggerAdapter{T}"/>.</param>
        /// <param name="weeWXDbProviderFactory">The <see cref="WeeWXRepositoryDbProviderFactory"/>.</param>
        /// <param name="weeWXRepositoryConfiguration">The config.</param>
        public WeeWXRepository(ILoggerAdapter<WeeWXRepository> logger, WeeWXRepositoryDbProviderFactory weeWXDbProviderFactory, IWeeWXRepositoryConfiguration weeWXRepositoryConfiguration)
        {
            if (weeWXDbProviderFactory == null)
            {
                throw new ArgumentNullException(nameof(weeWXDbProviderFactory));
            }

            if (weeWXRepositoryConfiguration == null)
            {
                throw new ArgumentNullException(nameof(weeWXRepositoryConfiguration));
            }

            _logger = logger;
            _weeWXDbProviderFactory = weeWXDbProviderFactory.WeeWXDbProviderFactory;
            _connectionString = weeWXRepositoryConfiguration.ConnectionString;
        }

        /// <inheritdoc/>
        public async Task<List<ObservationModel>> GetWeather(long startTimestamp, long endTimestamp)
        {
            var statement = @"
SELECT
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
FROM archive
WHERE
    dateTime>=@startTimestamp
    AND dateTime<@endTimestamp
";

            var observationsModel = new List<ObservationModel>();

            var dbConnection = _weeWXDbProviderFactory.CreateConnection();
            dbConnection.ConnectionString = _connectionString;
            using (dbConnection)
            {
                var dbCommand = dbConnection.CreateCommand();
                dbCommand.CommandText = statement;
                using (dbCommand)
                {
                    dbCommand.AddParamWithValue("@startTimestamp", startTimestamp);
                    dbCommand.AddParamWithValue("@endTimestamp", endTimestamp);

                    dbConnection.Open();

                    using (var rdr = dbCommand.ExecuteReader())
                    {
                        while (await rdr.ReadAsync().ConfigureAwait(true))
                        {
                            observationsModel.Add(this.ReadObservation(rdr));
                        }
                    }
                }
            }

            return observationsModel;
        }

        private ObservationModel ReadObservation(DbDataReader rdr)
        {
            return new ObservationModel
            {
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