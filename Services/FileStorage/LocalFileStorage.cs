using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Reflection.Metadata.Ecma335;

namespace MyHub.Services.FileStorage
{
    public class LocalFileStorage : IFileStorage
    {
        private readonly static string basePath = Path.Combine(System.IO.Directory.GetCurrentDirectory(), "LocalStorage");

        public Task DeleteAsync(string storageKey, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<Stream> DownloadAsync(string storageKey, CancellationToken cancellationToken = default)
        {
            var filePathSource = Path.Combine(basePath, storageKey);
            Stream fileStream = new FileStream(filePathSource, FileMode.Open, FileAccess.Read, FileShare.Read);
            return Task.FromResult(fileStream);
        }

        public async Task<string> UploadAsync(Stream stream, string fileName, string contentType, CancellationToken cancellationToken = default)
        {
            //Already contains extension
            var filePath = Path.Combine(basePath, fileName);
            var fileDestination = File.Create(filePath);

            var buffer = new byte[4096];
            int bytesRead = 0;
            try
            {
                while ((bytesRead = await stream.ReadAsync(buffer)) > 0)
                {
                    await fileDestination.WriteAsync(buffer.AsMemory(0, bytesRead), cancellationToken);
                }
            }
            catch(Exception e)
            {
                return await Task<string>.FromResult("");
            }
            return await Task<string>.FromResult(fileName);
        }
    }
}
