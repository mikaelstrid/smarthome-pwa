using System.Threading.Tasks;
using SmartHome.Pwa.Core.Models;

namespace SmartHome.Pwa.Core.Interfaces
{
    public interface ITemperatureHumidityRepository
    {
        Task<TemperatureHumidityReading> GetLatest(string sensorId);
    }
}
