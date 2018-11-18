using Microsoft.Extensions.Options;
using SmartHome.Pwa.Core.Interfaces;
using SmartHome.Pwa.Core.Models;
using SmartHome.Pwa.Infrastructure.Common;
using SmartHome.Pwa.Infrastructure.Configuration;
using SmartHome.Pwa.Infrastructure.TemperatureHumidity.Models;

namespace SmartHome.Pwa.Infrastructure.TemperatureHumidity
{
    public class TemperatureHumidityTableStorage : TableStorageBase<TemperatureHumidityReadingEntity, TemperatureHumidityReading>, ITemperatureHumidityRepository
    {
        public TemperatureHumidityTableStorage(IOptions<TableStorageOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;
        }

        protected override string TableName => "TemperatureHumidity";
    }
}
