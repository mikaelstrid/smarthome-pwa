using System;

namespace SmartHome.Pwa.Core.Models
{
    public class CurrentWeatherReport
    {
        public string City { get; set; }
        public DateTimeOffset TimestampUtc { get; set; }
        public double Temperature { get; set; }
        public int WeatherId { get; set; }
        public string WeatherDescription { get; set; }
        public string WeatherIcon { get; set; }
    }
}
