using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartHome.Pwa.Core.Models;
using SmartHome.Pwa.Core.Utilities;

namespace SmartHome.Pwa.Core.Interfaces
{
    public interface IClimateService
    {
        Task<DataResult<TemperatureHumidityReading>> GetLatestTemperatureHumidityReading(string sensorId);
        Task<DataResult<IEnumerable<AggregatedTemperatureHumidityReadings>>> GetTemperatureHumidityReadings(string sensorId, DateTimeOffset from, DateTimeOffset to);
    }
}
