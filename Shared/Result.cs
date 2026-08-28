using MyHub.Enums;
using MyHub.Shared.Models;

namespace MyHub.Shared
{
    public record Result
    {
        public bool IsSuccess;
        public Error? Error;
        public Result(bool isSuccess, Error? error)
        {
            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new Result(true, null);
        public static Result Failure(Error error) => new Result(false, error);

        public static implicit operator Result(Error error) => Failure(error);
    }
    public record Result<T> : Result
    {
        public T? Value;

        private Result(T? value) : base(true, null) => Value = value;
        private Result(Error error) : base(false, error) { }

        public static implicit operator Result<T>(T? value) => new Result<T>(value);
        public static implicit operator Result<T>(Error error) => new Result<T>(error);
    }
}
