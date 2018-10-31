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
            var result = ClimateService.CalculateIntervalLength(TimeSpan.FromSeconds(intervalLengthInSeconds));

            // ASSERT
            result.TotalSeconds.Should().Be(expectedResultInSeconds);
        }


        [Fact]
        public void CreateIntervalDataGroups_ShouldReturnEmptyArray_GivenNull()
        {
            // ARRANGE

            // ACT
            var result = ClimateService.CreateIntervalDataGroups(null, TimeSpan.FromMinutes(1));

            // ASSERT
            result.Should().BeEmpty();
        }

        [Fact]
        public void CreateIntervalDataGroups_ShouldReturnEmptyArray_GivenEmptyArray()
        {
            // ARRANGE

            // ACT
            var result = ClimateService.CreateIntervalDataGroups(new TemperatureHumidityReading[0], TimeSpan.FromMinutes(1));

            // ASSERT
            result.Should().BeEmpty();
        }

        [Fact]
        public void CreateIntervalDataGroups_ShouldReturnOneGroup_GivenOneReading()
        {
            // ARRANGE
            var readings = new[]
            {
                new TemperatureHumidityReading { Timestamp = DateTimeOffset.Parse("2018-10-30 20:18:17 +01:00") }
            };

            // ACT
            var result = ClimateService.CreateIntervalDataGroups(readings, TimeSpan.FromMinutes(1));

            // ASSERT
            result.Count().Should().Be(1);
            result.First().Count.Should().Be(1);
            result.First().From.Should().Be(DateTimeOffset.Parse("2018-10-30 20:18:00 +01:00"));
            result.First().To.Should().Be(DateTimeOffset.Parse("2018-10-30 20:19:00 +01:00"));
        }

        [Fact]
        public void CreateIntervalDataGroups_ShouldReturnOneGroup_GivenTwoReadingsWithinOneMinute()
        {
            // ARRANGE
            var readings = new[]
            {
                new TemperatureHumidityReading { Timestamp = DateTimeOffset.Parse("2018-10-30 20:18:17 +01:00") },
                new TemperatureHumidityReading { Timestamp = DateTimeOffset.Parse("2018-10-30 20:18:47 +01:00") }
            };

            // ACT
            var result = ClimateService.CreateIntervalDataGroups(readings, TimeSpan.FromMinutes(1));

            // ASSERT
            result.Count().Should().Be(1);
            result.First().Count.Should().Be(2);
            result.First().From.Should().Be(DateTimeOffset.Parse("2018-10-30 20:18:00 +01:00"));
            result.First().To.Should().Be(DateTimeOffset.Parse("2018-10-30 20:19:00 +01:00"));
        }

        [Fact]
        public void CreateIntervalDataGroups_ShouldReturnTwoGroups_GivenTwoReadings90SecondsApart()
        {
            // ARRANGE
            var readings = new[]
            {
                new TemperatureHumidityReading { Timestamp = DateTimeOffset.Parse("2018-10-30 20:18:17 +01:00") },
                new TemperatureHumidityReading { Timestamp = DateTimeOffset.Parse("2018-10-30 20:19:47 +01:00") }
            };

            // ACT
            var result = ClimateService.CreateIntervalDataGroups(readings, TimeSpan.FromMinutes(1));

            // ASSERT
            result.Count().Should().Be(2);
            result.First().Count.Should().Be(1);
            result.First().From.Should().Be(DateTimeOffset.Parse("2018-10-30 20:18:00 +01:00"));
            result.First().To.Should().Be(DateTimeOffset.Parse("2018-10-30 20:19:00 +01:00"));
            result.Skip(1).First().Count.Should().Be(1);
            result.Skip(1).First().From.Should().Be(DateTimeOffset.Parse("2018-10-30 20:19:00 +01:00"));
            result.Skip(1).First().To.Should().Be(DateTimeOffset.Parse("2018-10-30 20:20:00 +01:00"));
        }

        [Fact]
        public void CreateIntervalDataGroups_ShouldReturnTwoGroups_GivenTwoReadings150SecondsApart()
        {
            // ARRANGE
            var readings = new[]
            {
                new TemperatureHumidityReading { Timestamp = DateTimeOffset.Parse("2018-10-30 20:18:17 +01:00") },
                new TemperatureHumidityReading { Timestamp = DateTimeOffset.Parse("2018-10-30 20:20:47 +01:00") }
            };

            // ACT
            var result = ClimateService.CreateIntervalDataGroups(readings, TimeSpan.FromMinutes(1));

            // ASSERT
            result.Count().Should().Be(2);
            result.First().Count.Should().Be(1);
            result.First().From.Should().Be(DateTimeOffset.Parse("2018-10-30 20:18:00 +01:00"));
            result.First().To.Should().Be(DateTimeOffset.Parse("2018-10-30 20:19:00 +01:00"));
            result.Skip(1).First().Count.Should().Be(1);
            result.Skip(1).First().From.Should().Be(DateTimeOffset.Parse("2018-10-30 20:20:00 +01:00"));
            result.Skip(1).First().To.Should().Be(DateTimeOffset.Parse("2018-10-30 20:21:00 +01:00"));
        }

        [Fact]
        public void CreateIntervalDataGroups_ShouldReturnThreeGroups_GivenTwoReadings150SecondsApartInReverseOrder()
        {
            // ARRANGE
            var readings = new[]
            {
                new TemperatureHumidityReading { Timestamp = DateTimeOffset.Parse("2018-10-30 20:20:47 +01:00") },
                new TemperatureHumidityReading { Timestamp = DateTimeOffset.Parse("2018-10-30 20:18:17 +01:00") }
            };

            // ACT
            var result = ClimateService.CreateIntervalDataGroups(readings, TimeSpan.FromMinutes(1));

            // ASSERT
            result.Count().Should().Be(2);
            result.First().Count.Should().Be(1);
            result.First().From.Should().Be(DateTimeOffset.Parse("2018-10-30 20:18:00 +01:00"));
            result.First().To.Should().Be(DateTimeOffset.Parse("2018-10-30 20:19:00 +01:00"));
            result.Skip(1).First().Count.Should().Be(1);
            result.Skip(1).First().From.Should().Be(DateTimeOffset.Parse("2018-10-30 20:20:00 +01:00"));
            result.Skip(1).First().To.Should().Be(DateTimeOffset.Parse("2018-10-30 20:21:00 +01:00"));
        }

        [Fact]
        public void CreateIntervalDataGroups_ShouldReturnTwoGroups_GivenTwoReadings50MinutesApart_And15MinuteInterval()
        {
            // ARRANGE
            var readings = new[]
            {
                new TemperatureHumidityReading { Timestamp = DateTimeOffset.Parse("2018-10-30 20:01:07 +01:00") },
                new TemperatureHumidityReading { Timestamp = DateTimeOffset.Parse("2018-10-30 20:51:47 +01:00") }
            };

            // ACT
            var result = ClimateService.CreateIntervalDataGroups(readings, TimeSpan.FromMinutes(15));

            // ASSERT
            result.Count().Should().Be(2);
            result.First().Count.Should().Be(1);
            result.First().From.Should().Be(DateTimeOffset.Parse("2018-10-30 20:00:00 +01:00"));
            result.First().To.Should().Be(DateTimeOffset.Parse("2018-10-30 20:15:00 +01:00"));
            result.Skip(1).First().Count.Should().Be(1);
            result.Skip(1).First().From.Should().Be(DateTimeOffset.Parse("2018-10-30 20:45:00 +01:00"));
            result.Skip(1).First().To.Should().Be(DateTimeOffset.Parse("2018-10-30 21:00:00 +01:00"));
        }

        [Fact]
        public void CreateIntervalDataGroups_ShouldReturnTwoGroups_GivenThreeReadings59MinutesApart_And15MinuteInterval()
        {
            // ARRANGE
            var readings = new[]
            {
                new TemperatureHumidityReading { Timestamp = DateTimeOffset.Parse("2018-10-30 20:01:07 +01:00") },
                new TemperatureHumidityReading { Timestamp = DateTimeOffset.Parse("2018-10-30 20:51:47 +01:00") },
                new TemperatureHumidityReading { Timestamp = DateTimeOffset.Parse("2018-10-30 20:11:07 +01:00") }
            };

            // ACT
            var result = ClimateService.CreateIntervalDataGroups(readings, TimeSpan.FromMinutes(15));

            // ASSERT
            result.Count().Should().Be(2);
            result.First().Count.Should().Be(2);
            result.First().From.Should().Be(DateTimeOffset.Parse("2018-10-30 20:00:00 +01:00"));
            result.First().To.Should().Be(DateTimeOffset.Parse("2018-10-30 20:15:00 +01:00"));
            result.Skip(1).First().Count.Should().Be(1);
            result.Skip(1).First().From.Should().Be(DateTimeOffset.Parse("2018-10-30 20:45:00 +01:00"));
            result.Skip(1).First().To.Should().Be(DateTimeOffset.Parse("2018-10-30 21:00:00 +01:00"));
        }


        [Theory]
        [InlineData("2018-10-31 21:17:17 +01:00", 1, "2018-10-31 21:17:17 +01:00")]
        [InlineData("2018-10-31 21:17:17 +01:00", 30, "2018-10-31 21:17:00 +01:00")]
        [InlineData("2018-10-31 21:17:17 +01:00", 60, "2018-10-31 21:17:00 +01:00")]
        [InlineData("2018-10-31 21:17:17 +01:00", 60*5, "2018-10-31 21:15:00 +01:00")]
        [InlineData("2018-10-31 21:17:17 +01:00", 60 * 60 * 4, "2018-10-31 20:00:00 +01:00")]
        [InlineData("2018-10-31 21:17:17 +01:00", 60 * 60 * 24, "2018-10-31 00:00:00 +01:00")]
        public void CalculateIntervalStartTime_TestCase(string timestamp, int intervalLengthInSeconds, string expectedResult)
        {
            // ARRANGE

            // ACT
            var result = ClimateService.CalculateIntervalStartTime(DateTimeOffset.Parse(timestamp), TimeSpan.FromSeconds(intervalLengthInSeconds));

            // ASSERT
            result.Should().Be(DateTimeOffset.Parse(expectedResult));
        }
    }
}
