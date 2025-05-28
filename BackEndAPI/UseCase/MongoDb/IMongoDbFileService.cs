// Ignore Spelling: Mongo
using Microsoft.AspNetCore.Http;
using UseCase.Shared.Responses.ItemResponse.FileResponses;

namespace UseCase.MongoDb
{
    public interface IMongoDbFileService
    {
        Task<string> SaveAsync(IFormFile file, CancellationToken cancellationToken);

        Task<FileDto?> GetAsync(string fileId, CancellationToken cancellationToken);

        Task DeleteFileAsync(string fileId, CancellationToken cancellationToken);
    }
}
