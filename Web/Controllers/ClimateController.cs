using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SmartHome.Pwa.Core.Interfaces;
using SmartHome.Pwa.Web.Models;

namespace SmartHome.Pwa.Web.Controllers
{
    [Route("api/[controller]")]
    public class ClimateController : Controller
    {
        private readonly ITemperatureHumidityRepository _temperatureHumidityRepository;

        public ClimateController(ITemperatureHumidityRepository temperatureHumidityRepository)
        {
            _temperatureHumidityRepository = temperatureHumidityRepository;
        }

        [HttpGet("{sensorId}/temperature-humidity")]
        public async Task<IActionResult> GetTemperatureHumidity(string sensorId)
        {
            var result = await _temperatureHumidityRepository.GetLatest(sensorId);

            if (result == null)
                return NotFound();

            return Ok(result.ToApiModel());
        }
    }
}

//:TODO:
// * Gå igenom West/UTC (ändra även namngivning i Table Storage)
// * Lägg upp DateTimeHelpers på Github
// * Lägg upp Result-klasserna på Github
