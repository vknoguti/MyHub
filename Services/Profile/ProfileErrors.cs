using MyHub.Enums;
using MyHub.Extensions;
using MyHub.Shared.Models;

namespace MyHub.Services.Profile
{
    public static class ProfileErrors
    {
        public static Error UserNotFound { get; } = new Error(nameof(ErrorType.UserNotFound),
                                                      ErrorType.UserNotFound,
                                                      ErrorType.UserNotFound.GetDescriptionMessage() ?? string.Empty);

        public static Error ProfileAlreadyExists { get; } = new Error(nameof(ErrorType.ProfileAlreadyExists),
                                                      ErrorType.ProfileAlreadyExists,
                                                      ErrorType.ProfileAlreadyExists.GetDescriptionMessage() ?? string.Empty);

        public static Error MapProfileFailed { get; } = new Error(nameof(ErrorType.MapProfileFailed),
                                                      ErrorType.MapProfileFailed,
                                                      ErrorType.MapProfileFailed.GetDescriptionMessage() ?? string.Empty);

        public static Error FailedDatabaseUpdate { get; } = new Error(nameof(ErrorType.FailedDatabaseUpdate),
                                        ErrorType.FailedDatabaseUpdate,
                                        ErrorType.FailedDatabaseUpdate.GetDescriptionMessage() ?? string.Empty);

        public static Error ProfileNotFound { get; } = new Error(nameof(ErrorType.ProfileNotFound),
                                        ErrorType.ProfileNotFound,
                                        ErrorType.ProfileNotFound.GetDescriptionMessage() ?? string.Empty);
    }
}
