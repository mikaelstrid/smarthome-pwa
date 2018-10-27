using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SmartHome.Pwa.Core.Interfaces;
using SmartHome.Pwa.Core.Models;
using SmartHome.Pwa.Core.Utilities;
using SmartHome.Pwa.Infrastructure.Configuration;
using SmartHome.Pwa.Infrastructure.TemperatureHumidity.Models;
using System;
using System.Collections.Generic;
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
                var table = GetTableReference();

                var query = new TableQuery<TemperatureHumidityReadingEntity>()
                    .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, sensorId))
                    .Take(1);

                var result = (await table.ExecuteQuerySegmentedAsync(query, null)).FirstOrDefault();
                return result != null 
                    ? DataResult<TemperatureHumidityReading>.CreateSuccessResult(result.MapToBusinessModel()) 
                    : DataResult<TemperatureHumidityReading>.CreateNotFoundResult();
            }
            catch (Exception e)
            {
                return DataResult<TemperatureHumidityReading>.CreateErrorResult(new Error(ErrorCode.ExceptionThrown, e));
            }
        }

        public async Task<DataResult<IEnumerable<TemperatureHumidityReading>>> Get(string sensorId, DateTimeOffset from, DateTimeOffset to)
        {
            try
            {
                var table = GetTableReference();

                var fromInvertedTicks = ConvertDateTimeOffsetToInvertedTicksString(from);
                var toInvertedTicks = ConvertDateTimeOffsetToInvertedTicksString(to);

                var sensorQuery = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, sensorId);

                var fromToQuery = TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.LessThanOrEqual, fromInvertedTicks),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThanOrEqual, toInvertedTicks)
                );
                
                var query = new TableQuery<TemperatureHumidityReadingEntity>()
                    .Where(TableQuery.CombineFilters(sensorQuery, TableOperators.And, fromToQuery));

                var result = new List<TemperatureHumidityReading>();
                TableContinuationToken continuationToken = null;
                do
                {
                    var queryResult = await table.ExecuteQuerySegmentedAsync(query, continuationToken);
                    result.AddRange(queryResult.Results.Select(r => r.MapToBusinessModel()));
                    continuationToken = queryResult.ContinuationToken;
                } while (continuationToken != null);

                return DataResult<IEnumerable<TemperatureHumidityReading>>.CreateSuccessResult(result);
            }
            catch (Exception e)
            {
                return DataResult<IEnumerable<TemperatureHumidityReading>>.CreateErrorResult(new Error(ErrorCode.ExceptionThrown, e));
            }
        }


        private static string ConvertDateTimeOffsetToInvertedTicksString(DateTimeOffset input)
        {
            return $"{DateTimeOffset.MaxValue.Ticks - input.UtcTicks:D19}";
        }


        private CloudTable GetTableReference()
        {
            var storageAccount = CloudStorageAccount.Parse(_options.ConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference("TemperatureHumidity");
            return table;
        }
    }
}
