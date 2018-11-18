using System.Threading.Tasks;
using SmartHome.Pwa.Core.Models;
using SmartHome.Pwa.Core.Utilities;

namespace SmartHome.Pwa.Core.Interfaces
{
    public interface ICurrentWeatherRepository
    {
        Task<DataResult<CurrentWeatherReport>> GetLatest(string city);
    }
}