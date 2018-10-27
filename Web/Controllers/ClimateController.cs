using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartHome.Pwa.Core.Interfaces;
using SmartHome.Pwa.Core.Utilities;
using SmartHome.Pwa.Web.Models;

namespace SmartHome.Pwa.Web.Controllers
{
    [Route("api/[controller]")]
    public class ClimateController : Controller
    {
        private readonly IClimateService _climateService;

        public ClimateController(IClimateService climateService)
        {
            _climateService = climateService;
        }

        [HttpGet("{sensorId}/latest-temperature-humidity")]
        public async Task<IActionResult> GetLatestTemperatureHumidity(string sensorId)
        {
            var result = await _climateService.GetLatestTemperatureHumidityReading(sensorId);

            if (!result.IsSuccessful)
                return result.Error.ErrorCode == ErrorCode.NotFound ? NotFound() : StatusCode(500);

            return Ok(result.Data.ToApiModel());
        }

        [HttpGet("{sensorId}/temperature-humidity")]
        public async Task<IActionResult> GetTemperatureHumidity(string sensorId, DateTime from, DateTime to)
        {
            var result = await _climateService.GetTemperatureHumidityReadings(sensorId, DateTimeHelper.ConvertWestToUtcOffset(from), DateTimeHelper.ConvertWestToUtcOffset(to));

            if (!result.IsSuccessful)
                return result.Error.ErrorCode == ErrorCode.NotFound ? NotFound() : StatusCode(500);

            return Ok(result.Data.Select(r => r.ToApiModel()));
        }
    }
}
