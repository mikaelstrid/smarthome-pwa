using System;

namespace SmartHome.Pwa.Core.Models
{
    public class AggregatedTemperatureHumidityReadings
    {
        public int Count { get; set; }
        public DateTimeOffset FromUtc { get; set; }
        public DateTimeOffset ToUtc { get; set; }    
        public double AverageTemperature { get; set; }
        public double  AverageHumidity { get; set; }
    }
}