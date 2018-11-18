using Microsoft.Extensions.Options;
using SmartHome.Pwa.Core.Interfaces;
using SmartHome.Pwa.Core.Models;
using SmartHome.Pwa.Infrastructure.Common;
using SmartHome.Pwa.Infrastructure.Configuration;
using SmartHome.Pwa.Infrastructure.CurrentWeather.Models;

namespace SmartHome.Pwa.Infrastructure.CurrentWeather
{
    public class CurrentWeatherTableStorage : TableStorageBase<CurrentWeatherReportEntity, CurrentWeatherReport>, ICurrentWeatherRepository
    {
        public CurrentWeatherTableStorage(IOptions<TableStorageOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;
        }

        protected override string TableName => "CurrentWeather";
    }
}
