using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Weather.Api.Models
{
    /// <summary>
    /// The weather observation.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class ObservationModel
    {
        /// <summary>
        /// Gets or sets the date time.
        /// </summary>
        /// <value>
        /// The date time.
        /// </value>
        public int DateTime { get; set; }

        /// <summary>
        /// Gets or sets the us units.
        /// </summary>
        /// <value>
        /// The us units.
        /// </value>
        public int USUnits { get; set; }

        /// <summary>
        /// Gets or sets the interval.
        /// </summary>
        /// <value>
        /// The interval.
        /// </value>
        public int Interval { get; set; }

        /// <summary>
        /// Gets or sets the barometer.
        /// </summary>
        /// <value>
        /// The barometer.
        /// </value>
        public double? Barometer { get; set; }

        /// <summary>
        /// Gets or sets the pressure.
        /// </summary>
        /// <value>
        /// The pressure.
        /// </value>
        public double? Pressure { get; set; }

        /// <summary>
        /// Gets or sets the altimeter.
        /// </summary>
        /// <value>
        /// The altimeter.
        /// </value>
        public double? Altimeter { get; set; }

        /// <summary>
        /// Gets or sets the outside temperature.
        /// </summary>
        /// <value>
        /// The outside temperature.
        /// </value>
        public double? OutsideTemperature { get; set; }

        /// <summary>
        /// Gets or sets the outside humidity.
        /// </summary>
        /// <value>
        /// The outside humidity.
        /// </value>
        public double? OutsideHumidity { get; set; }

        /// <summary>
        /// Gets or sets the wind speed.
        /// </summary>
        /// <value>
        /// The wind speed.
        /// </value>
        public double? WindSpeed { get; set; }

        /// <summary>
        /// Gets or sets the wind direction.
        /// </summary>
        /// <value>
        /// The wind direction.
        /// </value>
        public double? WindDirection { get; set; }

        /// <summary>
        /// Gets or sets the wind gust.
        /// </summary>
        /// <value>
        /// The wind gust.
        /// </value>
        public double? WindGust { get; set; }

        /// <summary>
        /// Gets or sets the wind gust direction.
        /// </summary>
        /// <value>
        /// The wind gust direction.
        /// </value>
        public double? WindGustDirection { get; set; }

        /// <summary>
        /// Gets or sets the rain rate.
        /// </summary>
        /// <value>
        /// The rain rate.
        /// </value>
        public double? RainRate { get; set; }

        /// <summary>
        /// Gets or sets the rain.
        /// </summary>
        /// <value>
        /// The rain.
        /// </value>
        public double? Rain { get; set; }

        /// <summary>
        /// Gets or sets the dew point.
        /// </summary>
        /// <value>
        /// The dew point.
        /// </value>
        public double? DewPoint { get; set; }

        /// <summary>
        /// Gets or sets the windchill.
        /// </summary>
        /// <value>
        /// The windchill.
        /// </value>
        public double? Windchill { get; set; }

        /// <summary>
        /// Gets or sets the heat index.
        /// </summary>
        /// <value>
        /// The heat index.
        /// </value>
        public double? HeatIndex { get; set; }

        /// <summary>
        /// Gets or sets the Evapotranspiration.
        /// </summary>
        /// <value>
        /// The Evapotranspiration.
        /// </value>
        public double? Evapotranspiration { get; set; }

        /// <summary>
        /// Gets or sets the radiation.
        /// </summary>
        /// <value>
        /// The radiation.
        /// </value>
        public double? Radiation { get; set; }

        /// <summary>
        /// Gets or sets the Ultraviolet.
        /// </summary>
        /// <value>
        /// The Ultraviolet.
        /// </value>
        public double? Ultraviolet { get; set; }

        /// <summary>
        /// Gets or sets the extra temperature1.
        /// </summary>
        /// <value>
        /// The extra temperature1.
        /// </value>
        public double? ExtraTemperature1 { get; set; }

        /// <summary>
        /// Gets or sets the extra temperature2.
        /// </summary>
        /// <value>
        /// The extra temperature2.
        /// </value>
        public double? ExtraTemperature2 { get; set; }

        /// <summary>
        /// Gets or sets the extra temperature3.
        /// </summary>
        /// <value>
        /// The extra temperature3.
        /// </value>
        public double? ExtraTemperature3 { get; set; }

        /// <summary>
        /// Gets or sets the soil temperature1.
        /// </summary>
        /// <value>
        /// The soil temperature1.
        /// </value>
        public double? SoilTemperature1 { get; set; }

        /// <summary>
        /// Gets or sets the soil temperature2.
        /// </summary>
        /// <value>
        /// The soil temperature2.
        /// </value>
        public double? SoilTemperature2 { get; set; }

        /// <summary>
        /// Gets or sets the soil temperature3.
        /// </summary>
        /// <value>
        /// The soil temperature3.
        /// </value>
        public double? SoilTemperature3 { get; set; }

        /// <summary>
        /// Gets or sets the soil temperature4.
        /// </summary>
        /// <value>
        /// The soil temperature4.
        /// </value>
        public double? SoilTemperature4 { get; set; }

        /// <summary>
        /// Gets or sets the leaf temperature1.
        /// </summary>
        /// <value>
        /// The leaf temperature1.
        /// </value>
        public double? LeafTemperature1 { get; set; }

        /// <summary>
        /// Gets or sets the leaf temperature2.
        /// </summary>
        /// <value>
        /// The leaf temperature2.
        /// </value>
        public double? LeafTemperature2 { get; set; }

        /// <summary>
        /// Gets or sets the extra humidity1.
        /// </summary>
        /// <value>
        /// The extra humidity1.
        /// </value>
        public double? ExtraHumidity1 { get; set; }

        /// <summary>
        /// Gets or sets the extra humidity2.
        /// </summary>
        /// <value>
        /// The extra humidity2.
        /// </value>
        public double? ExtraHumidity2 { get; set; }

        /// <summary>
        /// Gets or sets the soil moisture1.
        /// </summary>
        /// <value>
        /// The soil moisture1.
        /// </value>
        public double? SoilMoisture1 { get; set; }

        /// <summary>
        /// Gets or sets the soil moisture2.
        /// </summary>
        /// <value>
        /// The soil moisture2.
        /// </value>
        public double? SoilMoisture2 { get; set; }

        /// <summary>
        /// Gets or sets the soil moisture3.
        /// </summary>
        /// <value>
        /// The soil moisture3.
        /// </value>
        public double? SoilMoisture3 { get; set; }

        /// <summary>
        /// Gets or sets the soil moisture4.
        /// </summary>
        /// <value>
        /// The soil moisture4.
        /// </value>
        public double? SoilMoisture4 { get; set; }

        /// <summary>
        /// Gets or sets the leaf wetness1.
        /// </summary>
        /// <value>
        /// The leaf wetness1.
        /// </value>
        public double? LeafWetness1 { get; set; }

        /// <summary>
        /// Gets or sets the leaf wetness2.
        /// </summary>
        /// <value>
        /// The leaf wetness2.
        /// </value>
        public double? LeafWetness2 { get; set; }
    }
}