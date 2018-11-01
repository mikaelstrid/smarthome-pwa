using System;

namespace SmartHome.Pwa.Core.Models
{
    public class AggregatedTemperatureHumidityReadings
    {
        public int Count { get; set; }
        public DateTimeOffset From { get; set; }
        public DateTimeOffset To { get; set; }    
        public double AverageTemperature { get; set; }
        public double  AverageHumidity { get; set; }
    }
}