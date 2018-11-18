using SmartHome.Pwa.Core.Interfaces;
using SmartHome.Pwa.Core.Models;
using SmartHome.Pwa.Core.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Pwa.Core.Services
{
    public class ClimateService : IClimateService
    {
        private readonly ITemperatureHumidityRepository _temperatureHumidityRepository;
        private readonly ICurrentWeatherRepository _currentWeatherRepository;

        public ClimateService(ITemperatureHumidityRepository temperatureHumidityRepository, ICurrentWeatherRepository currentWeatherRepository)
        {
            _temperatureHumidityRepository = temperatureHumidityRepository;
            _currentWeatherRepository = currentWeatherRepository;
        }


        public async Task<DataResult<TemperatureHumidityReading>> GetLatestTemperatureHumidityReading(string sensorId)
        {
            return await _temperatureHumidityRepository.GetLatest(sensorId);
        }

        public async Task<DataResult<IEnumerable<AggregatedTemperatureHumidityReadings>>> GetTemperatureHumidityReadings(string sensorId, DateTimeOffset from, DateTimeOffset to)
        {
            if (!(to > from))
            {
                return DataResult<IEnumerable<AggregatedTemperatureHumidityReadings>>.CreateErrorResult(new Error(ErrorCode.FailedWithoutException, null, "To must be after from"));
            }

            var readingsResult = await _temperatureHumidityRepository.Get(sensorId, from, to);
            if (!readingsResult.IsSuccessful)
            {
                return DataResult<IEnumerable<AggregatedTemperatureHumidityReadings>>.CreateErrorResult(readingsResult.Error);
            }

            var intervalDataGroups = SortDataIntoGroups(
                sensorId,
                readingsResult.Data,
                CalculateIntervalLength(to - from)
                );

            return DataResult<IEnumerable<AggregatedTemperatureHumidityReadings>>
                .CreateSuccessResult(intervalDataGroups.Select(g => g.MapToBusinessModel()));
        }

        public async Task<DataResult<CurrentWeatherReport>> GetCurrentWeatherReport(string city)
        {
            return await _currentWeatherRepository.GetLatest(city);
        }



        internal static TimeSpan CalculateIntervalLength(TimeSpan input)
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

        internal static IEnumerable<IntervalDataGroup> SortDataIntoGroups(string sensorId, IEnumerable<TemperatureHumidityReading> readings, TimeSpan intervalLength)
        {
            if (readings == null || !readings.Any())
            {
                return new IntervalDataGroup[0];
            }

            var dictionary = new SortedDictionary<long, IntervalDataGroup>();
            foreach (var reading in readings)
            {
                var intervalStartTime = CalculateIntervalStartTime(reading.TimestampUtc, intervalLength);
                var key = intervalStartTime.UtcTicks;
                if (!dictionary.ContainsKey(key))
                {
                    dictionary.Add(key, new IntervalDataGroup(sensorId, intervalStartTime, intervalLength, reading));
                }
                else
                {
                    dictionary[key].AddReading(reading);
                }
            }

            return dictionary.Values.ToArray();
        }

        internal static DateTimeOffset CalculateIntervalStartTime(DateTimeOffset timestamp, TimeSpan intervalLength)
        {
            if (intervalLength < TimeSpan.FromMinutes(1))
            {
                return new DateTimeOffset(
                    timestamp.Year,
                    timestamp.Month,
                    timestamp.Day,
                    timestamp.Hour,
                    timestamp.Minute,
                    timestamp.Second / intervalLength.Seconds * intervalLength.Seconds,
                    timestamp.Offset);
            }

            if (intervalLength < TimeSpan.FromHours(1))
            {
                return new DateTimeOffset(
                    timestamp.Year,
                    timestamp.Month,
                    timestamp.Day,
                    timestamp.Hour,
                    timestamp.Minute / intervalLength.Minutes * intervalLength.Minutes,
                    0,
                    timestamp.Offset);
            }

            if (intervalLength < TimeSpan.FromDays(1))
            {
                return new DateTimeOffset(
                    timestamp.Year,
                    timestamp.Month,
                    timestamp.Day,
                    timestamp.Hour / intervalLength.Hours * intervalLength.Hours,
                    0,
                    0,
                    timestamp.Offset);
            }

            return new DateTimeOffset(
                timestamp.Year,
                timestamp.Month,
                timestamp.Day / intervalLength.Days * intervalLength.Days,
                0,
                0,
                0,
                timestamp.Offset);
        }


        public class IntervalDataGroup
        {
            private string SensorId { get; }
            private double TotalTemperature { get; set; }
            private double TotalHumidity { get; set; }

            public DateTimeOffset From { get; }
            public DateTimeOffset To { get; }
            public int Count { get; private set; }

            public IntervalDataGroup(string sensorId, DateTimeOffset from, TimeSpan length, TemperatureHumidityReading reading)
            {
                SensorId = sensorId;
                From = from;
                To = from.Add(length);
                Count = 1;
                TotalTemperature = reading.Temperature;
                TotalHumidity = reading.Humidity;
            }

            public void AddReading(TemperatureHumidityReading reading)
            {
                Count++;
                TotalTemperature += reading.Temperature;
                TotalHumidity += reading.Humidity;
            }

            public AggregatedTemperatureHumidityReadings MapToBusinessModel()
            {
                return new AggregatedTemperatureHumidityReadings
                {
                    SensorId = SensorId,
                    From = From,
                    To = To,
                    Count = Count,
                    AverageTemperature = Count > 0 ? TotalTemperature / Count : 0,
                    AverageHumidity = Count > 0 ? TotalHumidity / Count : 0
                };
            }
        }
    }
}
