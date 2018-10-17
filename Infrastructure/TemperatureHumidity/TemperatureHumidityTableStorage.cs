﻿using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SmartHome.Pwa.Core.Interfaces;
using SmartHome.Pwa.Core.Models;
using SmartHome.Pwa.Core.Utilities;
using SmartHome.Pwa.Infrastructure.Configuration;
using SmartHome.Pwa.Infrastructure.TemperatureHumidity.Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SmartHome.Pwa.Infrastructure.TemperatureHumidity
{
    public class TemperatureHumidityTableStorage : ITemperatureHumidityRepository
    {
        private readonly TableStorageOptions _options;

        public TemperatureHumidityTableStorage(IOptions<TableStorageOptions> optionsAccessor)
        {
            _options = optionsAccessor.Value;
        }

        public async Task<DataResult<TemperatureHumidityReading>> GetLatest(string sensorId)
        {
            try
            {
                var storageAccount = CloudStorageAccount.Parse(_options.ConnectionString);
                var tableClient = storageAccount.CreateCloudTableClient();
                var table = tableClient.GetTableReference("TemperatureHumidity");

                var query = new TableQuery<TemperatureHumidityReadingEntity>()
                    .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, sensorId))
                    .Take(1);

                TemperatureHumidityReadingEntity result;
                TableContinuationToken continuationToken = null;
                do
                {
                    var employees = await table.ExecuteQuerySegmentedAsync(query, continuationToken);
                    result = employees.FirstOrDefault();
                    continuationToken = employees.ContinuationToken;
                } while (continuationToken != null);

                if (result == null)
                {
                    return DataResult<TemperatureHumidityReading>.CreateNotFoundResult();
                }

                return DataResult<TemperatureHumidityReading>.CreateSuccessResult(result?.MapToBusinessModel());
            }
            catch (Exception e)
            {
                return DataResult<TemperatureHumidityReading>.CreateErrorResult(new Error(ErrorCode.ExceptionThrown, e));
            }
        }
    }
}
