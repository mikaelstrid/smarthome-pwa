using FluentAssertions;
using SmartHome.Pwa.Core.Services;
using System;
using System.Linq;
using SmartHome.Pwa.Core.Models;
using Xunit;

namespace SmartHome.Pwa.Core.Tests.Services
{
    public class ClimateServiceTests
    {
        [Theory]
        [InlineData(60, 1)]
        [InlineData(120, 5)]
        [InlineData(15 * 60, 10)]
        [InlineData(30 * 60, 30)]
        [InlineData(100 * 60, 60)]
        [InlineData(400 * 60, 5 * 60)]
        [InlineData(7 * 24 * 3600, 4 * 3600)]
        [InlineData(30 * 24 * 3600, 12 * 3600)]
        [InlineData(90 * 24 * 3600, 24 * 3600)]
        [InlineData(180 * 24 * 3600, 24 * 3600)]
        public void CalculateInterval_Should(int intervalLengthInSeconds, int expectedResultInSeconds)
        {
            // ARRANGE

            // ACT
            var result = ClimateService.CalculateInterval(TimeSpan.FromSeconds(intervalLengthInSeconds));

            // ASSERT
            result.TotalSeconds.Should().Be(expectedResultInSeconds);
        }


        [Fact]
        public void CreateIntervalDataGroupsCase1()
        {
            // ARRANGE
            var readings = new []
            {
                new TemperatureHumidityReading()
            };

            // ACT
            var result = ClimateService.CreateIntervalDataGroups(readings, TimeSpan.FromMinutes(1));

            // ASSERT
            result.Count().Should().Be(1);
        }
    }
}
