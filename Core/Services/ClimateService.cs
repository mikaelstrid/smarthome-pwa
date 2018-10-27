using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SmartHome.Pwa.Core.Interfaces;
using SmartHome.Pwa.Core.Models;
using SmartHome.Pwa.Core.Utilities;

namespace SmartHome.Pwa.Core.Services
{
    public class ClimateService : IClimateService
    {
        private readonly ITemperatureHumidityRepository _temperatureHumidityRepository;

        public ClimateService(ITemperatureHumidityRepository temperatureHumidityRepository)
        {
            _temperatureHumidityRepository = temperatureHumidityRepository;
        }

        public async Task<DataResult<TemperatureHumidityReading>> GetLatestTemperatureHumidityReading(string sensorId)
        {
            return await _temperatureHumidityRepository.GetLatest(sensorId);
        }

        public async Task<DataResult<IEnumerable<TemperatureHumidityReading>>> GetTemperatureHumidityReadings(string sensorId, DateTimeOffset @from, DateTimeOffset to)
        {
            return await _temperatureHumidityRepository.Get(sensorId, from, to);
        }
    }
}
