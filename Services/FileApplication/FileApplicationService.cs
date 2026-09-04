using MyHub.Data;
using MyHub.Services.FileStorage;
using MyHub.Shared;

namespace MyHub.Services.FileApplication
{
    public class FileApplicationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IFileStorage _fileStorage;

        public FileApplicationService(ApplicationDbContext context, IFileStorage fileStorage)
        {
            _context = context;
            _fileStorage = fileStorage;
        }

        //public Task<Result> 
    }
}
