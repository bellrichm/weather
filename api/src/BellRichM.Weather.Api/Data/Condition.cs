using BellRichM.Attribute.CodeCoverage;

namespace BellRichM.Weather.Api.Data
{
    /// <summary>
    /// The min max weather conditions for a given time.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class Condition
    {
        /// <summary>
        /// Gets or sets the year.
        /// </summary>
        /// <value>
        /// The year.
        /// </value>
        public int Year { get; set; }

        /// <summary>
        /// Gets or sets the month.
        /// </summary>
        /// <value>
        /// The month.
        /// </value>
        public int Month { get; set; }

        /// <summary>
        /// Gets or sets the day.
        /// </summary>
        /// <value>
        /// The day.
        /// </value>
        public int Day { get; set; }

        /// <summary>
        /// Gets or sets the hour.
        /// </summary>
        /// <value>
        /// The hour.
        /// </value>
        public int Hour { get; set; }

        /// <summary>
        /// Gets or sets the minute.
        /// </summary>
        /// <value>
        /// The minute.
        /// </value>
        public int Minute { get; set; }

        /// <summary>
        /// Gets or sets the wind gust direction.
        /// </summary>
        /// <value>
        /// The wind gust direction.
        /// </value>
        public double? WindGustDirection { get; set; }

        /// <summary>
        /// Gets or sets the wind gust.
        /// </summary>
        /// <value>
        /// The wind gust.
        /// </value>
        public double? WindGust { get; set; }

        /// <summary>
        /// Gets or sets the wind direction.
        /// </summary>
        /// <value>
        /// The wind direction.
        /// </value>
        public double? WindDirection { get; set; }

        /// <summary>
        /// Gets or sets the wind speed.
        /// </summary>
        /// <value>
        /// The wind speed.
        /// </value>
        public double? WindSpeed { get; set; }

        /// <summary>
        /// Gets or sets the outside temperature.
        /// </summary>
        /// <value>
        /// The outside temperature.
        /// </value>
        public double? OutsideTemperature { get; set; }

        /// <summary>
        /// Gets or sets the heat index.
        /// </summary>
        /// <value>
        /// The heat index.
        /// </value>
        public double? HeatIndex { get; set; }

        /// <summary>
        /// Gets or sets the windchill.
        /// </summary>
        /// <value>
        /// The windchill.
        /// </value>
        public double? Windchill { get; set; }

        /// <summary>
        /// Gets or sets the dew point.
        /// </summary>
        /// <value>
        /// The dew point.
        /// </value>
        public double? DewPoint { get; set; }

        /// <summary>
        /// Gets or sets the barometer.
        /// </summary>
        /// <value>
        /// The barometer.
        /// </value>
        public double? Barometer { get; set; }

        /// <summary>
        /// Gets or sets the rain.
        /// </summary>
        /// <value>
        /// The rain.
        /// </value>
        public double? Rain { get; set; }

        /// <summary>
        /// Gets or sets the rain rate.
        /// </summary>
        /// <value>
        /// The rain rate.
        /// </value>
        public double? RainRate { get; set; }

        /// <summary>
        /// Gets or sets the outside humidity.
        /// </summary>
        /// <value>
        /// The outside humidity.
        /// </value>
        public double? OutsideHumidity { get; set; }
    }
}