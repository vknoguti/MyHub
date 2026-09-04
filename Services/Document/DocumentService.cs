using Microsoft.EntityFrameworkCore;
using MyHub.Data;
using MyHub.Entities;
using MyHub.Services.FileStorage;
using MyHub.Shared;
using System.Diagnostics;

namespace MyHub.Services.Document
{
    public class DocumentService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileStorage _fileStorage;
        public DocumentService(ApplicationDbContext context, IFileStorage fileStorage)
        {
            _context = context;
            _fileStorage = fileStorage;
        }

        public async Task<Result> UploadAsync(Guid profileId, IFormFile file)
        {
            var profile = await _context.Profiles.AsNoTracking().FirstOrDefaultAsync(t => t.Id == profileId);
            if(profile is null)
            {
                return DocumentErrors.ProfileNotFound;
            }

            string? storageKey;
            try
            {
                storageKey = await _fileStorage.UploadAsync(file.OpenReadStream(), Path.GetFileNameWithoutExtension(file.FileName), file.ContentType);
            }
            catch
            {
                return DocumentErrors.UploadDocumentFailed;
            }

            var document = new MyHub.Entities.Document
            {
                StorageKey = storageKey,
                ContentType = file.ContentType,
                FileName = Path.GetFileNameWithoutExtension(file.FileName),
                FileSizeBytes = file.Length,
                UploadedAt = DateTime.Now,
                ProfileId = profileId
            };
            await _context.Documents.AddAsync(document);
            var created = await _context.SaveChangesAsync();
            if (created <= 0)
            {
                return DocumentErrors.FailedDatabaseUpdate;
            }
            return Result.Success();
        }

        //public async Task<Result> DownloadAsync()
        //{

        //}
    }
}
