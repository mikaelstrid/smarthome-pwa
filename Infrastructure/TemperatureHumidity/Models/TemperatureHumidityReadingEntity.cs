using System;
using Microsoft.WindowsAzure.Storage.Table;
using SmartHome.Pwa.Core.Models;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace SmartHome.Pwa.Infrastructure.TemperatureHumidity.Models
{
    public class TemperatureHumidityReadingEntity : TableEntity
    {
        public DateTimeOffset ReadAtUtc { get; set; }
        public double Temperature { get; set; }
        public double Humidity { get; set; }

        public TemperatureHumidityReading MapToBusinessModel()
        {
            return new TemperatureHumidityReading
            {
                SensorId = PartitionKey,
                Timestamp = ReadAtUtc,
                Temperature = Temperature,
                Humidity = Humidity
            };
        }
    }
}
