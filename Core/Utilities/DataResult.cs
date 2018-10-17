namespace SmartHome.Pwa.Core.Utilities
{
    public class DataResult<TData> : Result
    {
        internal DataResult(TData data, Error error)
            : base(error)
        {
            Data = data;
        }

        internal DataResult(Error error)
            : base(error)
        {
            Data = default(TData);
        }

        public static DataResult<TData> CreateSuccessResult(TData data)
        {
            return new DataResult<TData>(data, null);
        }

        public new static DataResult<TData> CreateErrorResult(Error error)
        {
            return new DataResult<TData>(default(TData), error);
        }

        public new static DataResult<TData> CreateNotFoundResult(string message = "")
        {
            return new DataResult<TData>(default(TData), new Error(ErrorCode.NotFound, null, message));
        }

        public TData Data { get; }
    }
}