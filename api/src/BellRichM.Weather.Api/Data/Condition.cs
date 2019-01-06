namespace BellRichM.Weather.Api.Data
{
    /// <summary>
    /// The min max weather conditions for a given time.
    /// </summary>
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
        public int? Month { get; set; }

        /// <summary>
        /// Gets or sets the day.
        /// </summary>
        /// <value>
        /// The day.
        /// </value>
        public int? Day { get; set; }

        /// <summary>
        /// Gets or sets the hour.
        /// </summary>
        /// <value>
        /// The hour.
        /// </value>
        public int? Hour { get; set; }

        /// <summary>
        /// Gets or sets the maximum temperature.
        /// </summary>
        /// <value>
        /// The maximum temperature.
        /// </value>
        public string MaxTemp { get; set; }

        /// <summary>
        /// Gets or sets the minimum temperature.
        /// </summary>
        /// <value>
        /// The minimum temperature.
        /// </value>
        public string MinTemp { get; set; }

        /// <summary>
        /// Gets or sets the maximum humidity.
        /// </summary>
        /// <value>
        /// The maximum humidity.
        /// </value>
        public string MaxHumidity { get; set; }

        /// <summary>
        /// Gets or sets the minimum humidity.
        /// </summary>
        /// <value>
        /// The minimum humidity.
        /// </value>
        public string MinHumidity { get; set; }

        /// <summary>
        /// Gets or sets the maximum dewpoint.
        /// </summary>
        /// <value>
        /// The maximum dewpoint.
        /// </value>
        public string MaxDewpoint { get; set; }

        /// <summary>
        /// Gets or sets the minimum dewpoint.
        /// </summary>
        /// <value>
        /// The minimum dewpoint.
        /// </value>
        public string MinDewpoint { get; set; }

        /// <summary>
        /// Gets or sets the maximum index of the heat.
        /// </summary>
        /// <value>
        /// The maximum index of the heat.
        /// </value>
        public string MaxHeatIndex { get; set; }

        /// <summary>
        /// Gets or sets the minimum windchill.
        /// </summary>
        /// <value>
        /// The minimum windchill.
        /// </value>
        public string MinWindchill { get; set; }

        /// <summary>
        /// Gets or sets the maximum barometer.
        /// </summary>
        /// <value>
        /// The maximum barometer.
        /// </value>
        public string MaxBarometer { get; set; }

        /// <summary>
        /// Gets or sets the minimum barometer.
        /// </summary>
        /// <value>
        /// The minimum barometer.
        /// </value>
        public string MinBarometer { get; set; }

        /// <summary>
        /// Gets or sets the maximum ET.
        /// </summary>
        /// <value>
        /// The maximum ET.
        /// </value>
        public string MaxET { get; set; }

        /// <summary>
        /// Gets or sets the minimum ET.
        /// </summary>
        /// <value>
        /// The minimum ET.
        /// </value>
        public string MinET { get; set; }

        /// <summary>
        /// Gets or sets the maximum UV.
        /// </summary>
        /// <value>
        /// The maximum UV.
        /// </value>
        public string MaxUV { get; set; }

        /// <summary>
        /// Gets or sets the minimum UV.
        /// </summary>
        /// <value>
        /// The minimum UV.
        /// </value>
        public string MinUV { get; set; }

        /// <summary>
        /// Gets or sets the maximum radiation.
        /// </summary>
        /// <value>
        /// The maximum radiation.
        /// </value>
        public string MaxRadiation { get; set; }

        /// <summary>
        /// Gets or sets the minimum radiation.
        /// </summary>
        /// <value>
        /// The minimum radiation.
        /// </value>
        public string MinRadiation { get; set; }

        /// <summary>
        /// Gets or sets the maximum rain rate.
        /// </summary>
        /// <value>
        /// The maximum rain rate.
        /// </value>
        public string MaxRainRate { get; set; }

        /// <summary>
        /// Gets or sets the rain total.
        /// </summary>
        /// <value>
        /// The rain total.
        /// </value>
        public string RainTotal { get; set; }

        /// <summary>
        /// Gets or sets the maximum wind gust.
        /// </summary>
        /// <value>
        /// The maximum wind gust.
        /// </value>
        public string MaxWindGust { get; set; }

        /// <summary>
        /// Gets or sets the average wind speed.
        /// </summary>
        /// <value>
        /// The average wind speed.
        /// </value>
        public string AvgWindSpeed { get; set; }
    }
}