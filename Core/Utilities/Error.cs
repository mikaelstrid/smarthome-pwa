using System;

namespace SmartHome.Pwa.Core.Utilities
{
    public enum ErrorCode
    {
        Unknown = 0,
        ExceptionThrown = 1,
        FailedWithoutException = 2,
        NotFound = 3
    }

    public class Error
    {
        public Error(ErrorCode errorCode, Exception exception = null, string message = "")
        {
            ErrorCode = errorCode;
            Exception = exception;
            Message = message;
        }

        public ErrorCode ErrorCode { get; }

        public Exception Exception { get; }

        public string Message { get; }
    }
}