using System;
using SmartHome.Pwa.Core.Models;
using SmartHome.Pwa.Core.Utilities;

namespace SmartHome.Pwa.Web.Models
{
    public class CurrentWeatherApiModel
    {
        public string City { get; set; }
        public DateTime TimestampWest { get; set; }
        public int WeatherId { get; set; }
        public string WeatherDescription { get; set; }
        public string WeatherIcon { get; set; }
        public double Temperature { get; set; }
    }

    public static class CurrentWeatherApiModelExtensions
    {
        public static CurrentWeatherApiModel ToApiModel(this CurrentWeatherReport businessModel)
        {
            return new CurrentWeatherApiModel
            {
                City = businessModel.City,
                TimestampWest = DateTimeOffsetHelper.ConvertToWest(businessModel.TimestampUtc),
                WeatherId = businessModel.WeatherId,
                WeatherDescription = businessModel.WeatherDescription,
                WeatherIcon = businessModel.WeatherIcon,
                Temperature = businessModel.Temperature
            };
        }
    }
}
