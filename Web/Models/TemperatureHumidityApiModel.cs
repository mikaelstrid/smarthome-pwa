using System;
using SmartHome.Pwa.Core.Models;

namespace SmartHome.Pwa.Web.Models
{
    public class TemperatureHumidityApiModel
    {
        public string SensorId { get; set; }
        public DateTimeOffset TimestampWest { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }
    }

    public static class TemperatureHumidityApiModelExtensions
    {
        public static TemperatureHumidityApiModel ToApiModel(this TemperatureHumidityReading businessModel)
        {
            return new TemperatureHumidityApiModel
            {
                SensorId = businessModel.SensorId,
                TimestampWest = businessModel.TimestampUtc,
                Temperature = businessModel.Temperature,
                Humidity = businessModel.Humidity
            };
        }
    }
}
