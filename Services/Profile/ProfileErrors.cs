using MyHub.Enums;
using MyHub.Extensions;
using MyHub.Shared;
using MyHub.Shared.Models;

namespace MyHub.Services.Profile
{
    public static class ProfileErrors
    {
        public static readonly Error UserNotFound = new Error(nameof(ErrorType.UserNotFound),
                                                      ErrorType.UserNotFound,
                                                      ErrorType.UserNotFound.GetDescriptionMessage() ?? string.Empty);

        public static readonly Error ProfileAlreadyExists = new Error(nameof(ErrorType.ProfileAlreadyExists),
                                                      ErrorType.ProfileAlreadyExists,
                                                      ErrorType.ProfileAlreadyExists.GetDescriptionMessage() ?? string.Empty);

        public static readonly Error MapProfileFailed = new Error(nameof(ErrorType.MapProfileFailed),
                                                      ErrorType.MapProfileFailed,
                                                      ErrorType.MapProfileFailed.GetDescriptionMessage() ?? string.Empty);

        public static readonly Error FailedDatabaseUpdate = new Error(nameof(ErrorType.FailedDatabaseUpdate),
                                        ErrorType.FailedDatabaseUpdate,
                                        ErrorType.FailedDatabaseUpdate.GetDescriptionMessage() ?? string.Empty);

        public static readonly Error ProfileNotFound = new Error(nameof(ErrorType.ProfileNotFound),
                                        ErrorType.ProfileNotFound,
                                        ErrorType.ProfileNotFound.GetDescriptionMessage() ?? string.Empty);
    }
}
