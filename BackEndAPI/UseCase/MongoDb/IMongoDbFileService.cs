// Ignore Spelling: Mongo
using Microsoft.AspNetCore.Http;

namespace UseCase.MongoDb
{
    public interface IMongoDbFileService
    {
        Task<string> SaveAsync(IFormFile file, CancellationToken cancellationToken);

        Task<(Stream stream, string fileName)?> GetAsync(string fileId, CancellationToken cancellationToken);

        Task DeleteFileAsync(string fileId, CancellationToken cancellationToken);
    }
}
