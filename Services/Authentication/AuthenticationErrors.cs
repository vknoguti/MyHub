using MyHub.Enums;
using MyHub.Extensions;
using MyHub.Shared;
using MyHub.Shared.Models;

namespace MyHub.Services.Authentication
{
    public static class AuthenticationErrors
    {
        public static readonly Error UsernameAlreadyExists = new Error(nameof(ErrorType.UsernameAlreadyExists), 
                                                                       ErrorType.UsernameAlreadyExists, 
                                                                       ErrorType.UsernameAlreadyExists.GetDescriptionMessage() ?? string.Empty);

        public static readonly Error EmailAlreadyExists = new Error(nameof(ErrorType.EmailAlreadyExists),
                                                                    ErrorType.EmailAlreadyExists, 
                                                                    ErrorType.EmailAlreadyExists.GetDescriptionMessage() ?? string.Empty);


        public static readonly Error UserRegistrationFailed = new Error(nameof(ErrorType.UserRegistrationFailed),
                                                                    ErrorType.UserRegistrationFailed,
                                                                    ErrorType.UserRegistrationFailed.GetDescriptionMessage() ?? string.Empty);

        public static readonly Error UserNotFound = new Error(nameof(ErrorType.UserNotFound),
                                                      ErrorType.UserNotFound,
                                                      ErrorType.UserNotFound.GetDescriptionMessage() ?? string.Empty);

        public static readonly Error InvalidPassword = new Error(nameof(ErrorType.InvalidPassword),
                                                     ErrorType.InvalidPassword,
                                                     ErrorType.InvalidPassword.GetDescriptionMessage() ?? string.Empty);

        public static readonly Error NullRefreshToken = new Error(nameof(ErrorType.NullRefreshToken),
                                                     ErrorType.NullRefreshToken,
                                                     ErrorType.NullRefreshToken.GetDescriptionMessage() ?? string.Empty);

        public static readonly Error InvalidRefreshToken = new Error(nameof(ErrorType.InvalidRefreshToken),
                                                     ErrorType.InvalidRefreshToken,
                                                     ErrorType.InvalidRefreshToken.GetDescriptionMessage() ?? string.Empty);

        public static readonly Error UserAlreadyLoggedOut = new Error(nameof(ErrorType.UserAlreadyLoggedOut),
                                             ErrorType.UserAlreadyLoggedOut,
                                             ErrorType.UserAlreadyLoggedOut.GetDescriptionMessage() ?? string.Empty);

        public static readonly Error FailedDatabaseUpdate = new Error(nameof(ErrorType.FailedDatabaseUpdate),
                                        ErrorType.FailedDatabaseUpdate,
                                        ErrorType.FailedDatabaseUpdate.GetDescriptionMessage() ?? string.Empty);

    }
}
