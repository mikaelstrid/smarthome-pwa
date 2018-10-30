using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<DataResult<IEnumerable<AggregatedTemperatureHumidityReadings>>> GetTemperatureHumidityReadings(string sensorId, DateTimeOffset from, DateTimeOffset to)
        {
            if (!(to > from))
                return DataResult<IEnumerable<AggregatedTemperatureHumidityReadings>>.CreateErrorResult(new Error(ErrorCode.FailedWithoutException, null, "To must be after from"));

            var readingsResult = await _temperatureHumidityRepository.Get(sensorId, from, to);
            if (!readingsResult.IsSuccessful)
                return DataResult<IEnumerable<AggregatedTemperatureHumidityReadings>>.CreateErrorResult(readingsResult.Error);

            var intervalLength = CalculateInterval(to - from);
            var intervalDataGroups = CreateIntervalDataGroups(readingsResult.Data, intervalLength);

            return DataResult<IEnumerable<AggregatedTemperatureHumidityReadings>>.CreateSuccessResult(new List<AggregatedTemperatureHumidityReadings>());
        }

        internal static TimeSpan CalculateInterval(TimeSpan input)
        {
            var intervalLengths = new[]
            {
                TimeSpan.FromSeconds(1),
                TimeSpan.FromSeconds(5),
                TimeSpan.FromSeconds(10),
                TimeSpan.FromSeconds(15),
                TimeSpan.FromSeconds(30),
                TimeSpan.FromMinutes(1),
                TimeSpan.FromMinutes(5),
                TimeSpan.FromMinutes(10),
                TimeSpan.FromMinutes(15),
                TimeSpan.FromMinutes(30),
                TimeSpan.FromHours(1),
                TimeSpan.FromHours(4),
                TimeSpan.FromHours(6),
                TimeSpan.FromHours(12)
            };

            var result = intervalLengths
                .Where(l => l.TotalSeconds * 100 >= input.TotalSeconds)
                .OrderBy(l => l.TotalSeconds)
                .ToArray();

            return result.Any() ? result.FirstOrDefault() : TimeSpan.FromHours(24);
        }

        internal static IEnumerable<IntervalDataGroup> CreateIntervalDataGroups(IEnumerable<TemperatureHumidityReading> readings, TimeSpan intervalLength)
        {
            throw new NotImplementedException();
        }
    }

    internal class IntervalDataGroup
    {
    }
}
