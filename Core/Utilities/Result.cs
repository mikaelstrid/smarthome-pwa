using System;
using System.Globalization;
using System.Reflection;

namespace SmartHome.Pwa.Core.Utilities
{
    // Inspired by https://github.com/KathleenDollard/ClassTracker
    public class Result
    {
        internal Result(Error error)
        {
            Error = error;
            IsSuccessful = Error == null;
        }


        public bool IsSuccessful { get; }

        public Error Error { get; }


        public static Result CreateSuccessResult()
        {
            return new Result(null);
        }

        public static Result CreateErrorResult(Error error)
        {
            return new Result(error);
        }


        public static Result CreateNotFoundResult(string message = "")
        {
            return new Result(new Error(ErrorCode.NotFound, null, message));
        }

        public static TResult CreateSuccessResult<TResult>() where TResult : Result
            => CreateInstanceWithPublicOrNonPublicConstructor<TResult>(null, null);

        public static TResult CreateErrorResult<TResult>(Error error) where TResult : Result
            => CreateInstanceWithPublicOrNonPublicConstructor<TResult>(error);


        private static T CreateInstanceWithPublicOrNonPublicConstructor<T>(params object[] parameters)
        {
            var bindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            var obj = Activator.CreateInstance(typeof(T), bindingFlags, null, parameters, CultureInfo.CurrentCulture);
            return (T)obj;
        }
    }
}