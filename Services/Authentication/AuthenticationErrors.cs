using MyHub.Enums;
using MyHub.Extensions;
using MyHub.Shared.Models;

namespace MyHub.Services.Authentication
{
    public static class AuthenticationErrors
    {
        public static Error UsernameAlreadyExists { get; } = new Error(nameof(ErrorType.UsernameAlreadyExists), 
                                                                       ErrorType.UsernameAlreadyExists, 
                                                                       ErrorType.UsernameAlreadyExists.GetDescriptionMessage() ?? string.Empty);

        public static Error EmailAlreadyExists { get; } = new Error(nameof(ErrorType.EmailAlreadyExists),
                                                                    ErrorType.EmailAlreadyExists, 
                                                                    ErrorType.EmailAlreadyExists.GetDescriptionMessage() ?? string.Empty);


        public static Error UserRegistrationFailed { get; } = new Error(nameof(ErrorType.UserRegistrationFailed),
                                                                    ErrorType.UserRegistrationFailed,
                                                                    ErrorType.UserRegistrationFailed.GetDescriptionMessage() ?? string.Empty);

        public static Error UserNotFound { get; } = new Error(nameof(ErrorType.UserNotFound),
                                                      ErrorType.UserNotFound,
                                                      ErrorType.UserNotFound.GetDescriptionMessage() ?? string.Empty);

        public static Error InvalidPassword { get; } = new Error(nameof(ErrorType.InvalidPassword),
                                                     ErrorType.InvalidPassword,
                                                     ErrorType.InvalidPassword.GetDescriptionMessage() ?? string.Empty);

        public static Error NullRefreshToken { get; } = new Error(nameof(ErrorType.NullRefreshToken),
                                                     ErrorType.NullRefreshToken,
                                                     ErrorType.NullRefreshToken.GetDescriptionMessage() ?? string.Empty);

        public static Error InvalidRefreshToken { get; } = new Error(nameof(ErrorType.InvalidRefreshToken),
                                                     ErrorType.InvalidRefreshToken,
                                                     ErrorType.InvalidRefreshToken.GetDescriptionMessage() ?? string.Empty);

        public static Error UserAlreadyLoggedOut { get; } = new Error(nameof(ErrorType.UserAlreadyLoggedOut),
                                             ErrorType.UserAlreadyLoggedOut,
                                             ErrorType.UserAlreadyLoggedOut.GetDescriptionMessage() ?? string.Empty);

        public static Error FailedDatabaseUpdate { get; } = new Error(nameof(ErrorType.FailedDatabaseUpdate),
                                        ErrorType.FailedDatabaseUpdate,
                                        ErrorType.FailedDatabaseUpdate.GetDescriptionMessage() ?? string.Empty);

    }
}
