using System;
using Microsoft.WindowsAzure.Storage.Table;
using SmartHome.Pwa.Core.Models;
using SmartHome.Pwa.Infrastructure.Common;

// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

namespace SmartHome.Pwa.Infrastructure.CurrentWeather.Models
{
    public class CurrentWeatherReportEntity : TableEntity, IMappableToBusinessModel<CurrentWeatherReport>
    {
        public DateTimeOffset ReadAtUtc { get; set; }
        public int WeatherId { get; set; }
        public string WeatherDescription { get; set; }
        public string WeatherIcon { get; set; }
        public double Temperature { get; set; }

        public CurrentWeatherReport MapToBusinessModel()
        {
            return new CurrentWeatherReport
            {
                City = PartitionKey,
                TimestampUtc = ReadAtUtc,
                WeatherId = WeatherId,
                WeatherDescription = WeatherDescription,
                WeatherIcon = WeatherIcon,
                Temperature = Temperature,
            };
        }
    }
}
