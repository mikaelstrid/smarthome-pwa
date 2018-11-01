using System;
using SmartHome.Pwa.Core.Models;
using SmartHome.Pwa.Core.Utilities;

namespace SmartHome.Pwa.Web.Models
{
    public class AggregatedTemperatureHumidityApiModel
    {
        public int Count { get; set; }
        public DateTime FromWest { get; set; }
        public DateTime ToWest { get; set; }    
        public double AverageTemperature { get; set; }
        public double  AverageHumidity { get; set; }
    }

    public static class AggregatedTemperatureHumidityApiModelExtensions
    {
        public static AggregatedTemperatureHumidityApiModel ToApiModel(this AggregatedTemperatureHumidityReadings businessModel)
        {
            return new AggregatedTemperatureHumidityApiModel
            {
                Count = businessModel.Count,
                FromWest = DateTimeOffsetHelper.ConvertToWest(businessModel.From),
                ToWest = DateTimeOffsetHelper.ConvertToWest(businessModel.To),
                AverageTemperature = businessModel.AverageTemperature,
                AverageHumidity = businessModel.AverageHumidity
            };
        }
    }
}