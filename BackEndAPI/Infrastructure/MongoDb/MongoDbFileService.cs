using Domain.Shared.CustomProviders;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using UseCase.MongoDb;

namespace Infrastructure.MongoDb
{
    public class MongoDbFileService : IMongoDbFileService
    {
        // Properties
        private readonly string _recruitmentsCollectionName = "recruitments";
        private readonly IMongoDatabase _database;
        private readonly GridFSBucket _gridFSBucket;


        // Constructor
        public MongoDbFileService(IMongoDatabase database)
        {
            _database = database;

            var options = new GridFSBucketOptions
            {
                BucketName = _recruitmentsCollectionName
            };
            _gridFSBucket = new GridFSBucket(_database, options);
        }


        // Methods
        public async Task<string> SaveAsync(IFormFile file, CancellationToken cancellationToken)
        {
            using (var stream = file.OpenReadStream())
            {
                var options = new GridFSUploadOptions
                {
                    Metadata = new BsonDocument
                    {
                        { "contentType", file.ContentType },
                        { "originalFileName", file.FileName },
                        { "uploadDate", BsonDateTime.Create(CustomTimeProvider.Now) }
                    }
                };

                var fileId = await _gridFSBucket.UploadFromStreamAsync(file.FileName, stream, options);
                return fileId.ToString();
            }
        }

        public async Task<(Stream stream, string fileName)?> GetAsync(string fileId, CancellationToken cancellationToken)
        {
            if (!ObjectId.TryParse(fileId, out var objectId))
            {
                return null;
            }

            var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", objectId);
            var fileInfo = await _gridFSBucket.Find(filter).FirstOrDefaultAsync(cancellationToken);


            if (fileInfo == null)
            {
                return null;
            }

            var memoryStream = new MemoryStream();
            await _gridFSBucket.DownloadToStreamAsync(objectId, memoryStream);

            memoryStream.Position = 0;
            var fileName = fileInfo.Filename;

            return (memoryStream, fileName);
        }

        public async Task DeleteFileAsync(string fileId, CancellationToken cancellationToken)
        {
            if (ObjectId.TryParse(fileId, out var objectId))
            {
                await _gridFSBucket.DeleteAsync(objectId, cancellationToken);
            }
        }
    }
}
