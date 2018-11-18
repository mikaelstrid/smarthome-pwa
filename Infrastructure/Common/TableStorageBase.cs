using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Table;
using SmartHome.Pwa.Core.Utilities;
using SmartHome.Pwa.Infrastructure.Configuration;

namespace SmartHome.Pwa.Infrastructure.Common
{
    public abstract class TableStorageBase<T1, T2> where T1 : ITableEntity, IMappableToBusinessModel<T2>, new()
    {
        protected abstract string TableName { get; }

        protected TableStorageOptions _options;


        public async Task<DataResult<T2>> GetLatest(string partitionKey)
        {
            try
            {
                var table = GetTableReference(TableName);

                var query = new TableQuery<T1>()
                    .Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey))
                    .Take(1);

                var result = (await table.ExecuteQuerySegmentedAsync(query, null)).FirstOrDefault();
                return result != null 
                    ? DataResult<T2>.CreateSuccessResult(result.MapToBusinessModel()) 
                    : DataResult<T2>.CreateNotFoundResult();
            }
            catch (Exception e)
            {
                return DataResult<T2>.CreateErrorResult(new Error(ErrorCode.ExceptionThrown, e));
            }
        }

        public async Task<DataResult<IEnumerable<T2>>> Get(string partitionKey, DateTimeOffset from, DateTimeOffset to)
        {
            try
            {
                var table = GetTableReference(TableName);

                var fromInvertedTicks = ConvertDateTimeOffsetToInvertedTicksString(from);
                var toInvertedTicks = ConvertDateTimeOffsetToInvertedTicksString(to);

                var sensorQuery = TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey);

                var fromToQuery = TableQuery.CombineFilters(
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.LessThanOrEqual, fromInvertedTicks),
                    TableOperators.And,
                    TableQuery.GenerateFilterCondition("RowKey", QueryComparisons.GreaterThanOrEqual, toInvertedTicks)
                );
                
                var query = new TableQuery<T1>()
                    .Where(TableQuery.CombineFilters(sensorQuery, TableOperators.And, fromToQuery));

                var result = new List<T2>();
                TableContinuationToken continuationToken = null;
                do
                {
                    var queryResult = await table.ExecuteQuerySegmentedAsync(query, continuationToken);
                    result.AddRange(queryResult.Results.Select(r => r.MapToBusinessModel()));
                    continuationToken = queryResult.ContinuationToken;
                } while (continuationToken != null);

                return DataResult<IEnumerable<T2>>.CreateSuccessResult(result);
            }
            catch (Exception e)
            {
                return DataResult<IEnumerable<T2>>.CreateErrorResult(new Error(ErrorCode.ExceptionThrown, e));
            }
        }

        
        private static string ConvertDateTimeOffsetToInvertedTicksString(DateTimeOffset input)
        {
            return $"{DateTimeOffset.MaxValue.Ticks - input.UtcTicks:D19}";
        }

        private CloudTable GetTableReference(string tableName)
        {
            var storageAccount = CloudStorageAccount.Parse(_options.ConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient();
            var table = tableClient.GetTableReference(tableName);
            return table;
        }
    }
}