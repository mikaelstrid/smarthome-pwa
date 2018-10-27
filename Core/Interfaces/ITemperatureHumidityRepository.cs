using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartHome.Pwa.Core.Models;
using SmartHome.Pwa.Core.Utilities;

namespace SmartHome.Pwa.Core.Interfaces
{
    public interface ITemperatureHumidityRepository
    {
        Task<DataResult<TemperatureHumidityReading>> GetLatest(string sensorId);
        Task<DataResult<IEnumerable<TemperatureHumidityReading>>> Get(string sensorId, DateTimeOffset from, DateTimeOffset to);
    }
}
