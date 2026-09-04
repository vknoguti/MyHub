using MyHub.Extensions;
using MyHub.Shared;
using MyHub.Shared.Models;

namespace MyHub.Services.Document
{
    public static class DocumentErrors
    {

        public static readonly Error ProfileNotFound = new Error(nameof(ErrorType.ProfileNotFound),
                                                        ErrorType.ProfileNotFound,
                                                        ErrorType.ProfileNotFound.GetDescriptionMessage() ?? string.Empty);

        public static readonly Error UploadDocumentFailed = new Error(nameof(ErrorType.UploadDocumentFailed),
                                                                ErrorType.UploadDocumentFailed,
                                                                ErrorType.UploadDocumentFailed.GetDescriptionMessage() ?? string.Empty);

        public static readonly Error FailedDatabaseUpdate = new Error(nameof(ErrorType.FailedDatabaseUpdate),
                                                                ErrorType.FailedDatabaseUpdate,
                                                                ErrorType.FailedDatabaseUpdate.GetDescriptionMessage() ?? string.Empty);
    }
}
