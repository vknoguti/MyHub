using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using MyHub.Data;
using MyHub.Extensions;
using MyHub.Shared;
using System.Diagnostics;
using System.Reflection.Metadata.Ecma335;

namespace MyHub.Services.FileStorage
{
    public class LocalFileStorage : IFileStorage
    {

        private readonly ApplicationDbContext _context;
        private readonly static string basePath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "LocalStorage");

        public LocalFileStorage(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task DeleteAsync(string storageKey, CancellationToken cancellationToken = default)
        {
            string filePath = Path.Combine(basePath, storageKey);
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File not found");
            }

            await using var stream = new FileStream(
                filePath,
                FileMode.Open,
                FileAccess.Read,
                FileShare.None,
                bufferSize: 1,
                options: FileOptions.DeleteOnClose | FileOptions.Asynchronous);
        }

        public Task<Stream> DownloadAsync(string storageKey, CancellationToken cancellationToken = default)
        {
            var filePathSource = Path.Combine(basePath, storageKey);
            Stream fileStream = new FileStream(filePathSource, FileMode.Open, FileAccess.Read, FileShare.Read);
            return Task.FromResult(fileStream);
        }

        public async Task<string> UploadAsync(Stream stream, string fileName, string contentType, CancellationToken cancellationToken = default)
        {
            string extension = MimeTypeExtension.GetExtensionFromMimeType(contentType)
                ?? throw new FormatException("Content type not valid for operation");

            var currDateString = DateTime.Now.ToString("yyyy/MM/dd");
            var absoluteFolderPath = Path.Combine(basePath, currDateString);

            if (!Directory.Exists(absoluteFolderPath))
            {
                Directory.CreateDirectory(absoluteFolderPath);
            }

            var uniqueFileName = $"{Guid.NewGuid()}_{fileName}{extension}";

            var storageKey = Path.Combine(currDateString, uniqueFileName);
            var absoluteFilePath = Path.Combine(basePath, storageKey);

            try
            {
                await using var fileDestination = File.Create(absoluteFilePath); 
                await stream.CopyToAsync(fileDestination, cancellationToken);
            }
            catch (Exception ex)
            {
                throw new IOException("Occurred a problem during writing of file", ex);
            }
            return storageKey;
        }
    }
}
