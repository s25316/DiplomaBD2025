// Ignore Spelling: Mongo
using Microsoft.AspNetCore.Http;
using UseCase.MongoDb.Enums;
using UseCase.Shared.Responses.ItemResponse.FileResponses;

namespace UseCase.MongoDb
{
    public interface IMongoDbFileService
    {
        Task<string> SaveAsync(
            IFormFile file,
            MongoDbCollection collection = MongoDbCollection.Recruitments,
            CancellationToken cancellationToken = default);

        Task<FileDto?> GetAsync(
            string fileId,
            MongoDbCollection collection = MongoDbCollection.Recruitments,
            CancellationToken cancellationToken = default);

        Task DeleteFileAsync(
            string fileId,
            MongoDbCollection collection = MongoDbCollection.Recruitments,
            CancellationToken cancellationToken = default);
    }
}
