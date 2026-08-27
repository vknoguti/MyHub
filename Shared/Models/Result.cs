using System.Runtime.CompilerServices;

namespace MyHub.Shared.Models
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

        private Result(T value) : base(true, null) => Value = value;
        private Result(Error error) : base(false, error) { }

        public static implicit operator Result<T>(T value) => new Result<T>(value);
        public static implicit operator Result<T>(Error error) => new Result<T>(error);
    }


    public enum ErrorType { NotFound, Validation, Unauthorized }

    public record Error(string Id, ErrorType type, string description);

    public record AuthErrors(string id, ErrorType type, string description) : Error(id, type, description)
    {
    }
    public record ProfileErrors(string id, ErrorType type, string description) : Error(id, type, description);
    public record UserErrors(string id, ErrorType type, string description) : Error(id, type, description);
    public record TaskErrors(string id, ErrorType type, string description) : Error(id, type, description);

}
