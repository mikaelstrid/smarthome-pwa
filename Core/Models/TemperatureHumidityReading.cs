using System;

namespace SmartHome.Pwa.Core.Models
{
    public class TemperatureHumidityReading
    {
        public string SensorId { get; set; }
        public DateTimeOffset TimestampUtc { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
    }
}
