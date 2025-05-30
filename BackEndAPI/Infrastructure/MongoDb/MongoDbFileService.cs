using Domain.Shared.CustomProviders;
using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using UseCase.MongoDb;
using UseCase.MongoDb.Enums;
using UseCase.Shared.Responses.ItemResponse.FileResponses;

namespace Infrastructure.MongoDb
{
    public class MongoDbFileService : IMongoDbFileService
    {
        // Properties
        private readonly string _recruitmentsCollectionName = "recruitments";
        private readonly string _companyLogoCollectionName = "company-logo";
        private readonly IMongoDatabase _database;
        private readonly Dictionary<MongoDbCollection, IGridFSBucket> _buckets;


        // Constructor
        public MongoDbFileService(IMongoDatabase database)
        {
            _database = database;

            _buckets = new Dictionary<MongoDbCollection, IGridFSBucket>
            {
                {
                    MongoDbCollection.CompanyLogo,
                    new GridFSBucket(_database, new GridFSBucketOptions
                    {
                        BucketName = _companyLogoCollectionName
                    })
                },
                {
                    MongoDbCollection.Recruitments,
                    new GridFSBucket(_database, new GridFSBucketOptions
                    {
                        BucketName = _recruitmentsCollectionName
                    })
                }
            };
        }


        // Methods
        public async Task<string> SaveAsync(
            IFormFile file,
            MongoDbCollection collection = MongoDbCollection.Recruitments,
            CancellationToken cancellationToken = default)
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

                if (!_buckets.TryGetValue(collection, out var gridFSBucket))
                {
                    throw new ArgumentException($"No GridFS bucket configured for collection: {collection}");
                }

                var fileId = await gridFSBucket.UploadFromStreamAsync(file.FileName, stream, options);
                return fileId.ToString();
            }
        }

        public async Task<FileDto?> GetAsync(
            string fileId,
            MongoDbCollection collection = MongoDbCollection.Recruitments,
            CancellationToken cancellationToken = default)
        {
            if (!ObjectId.TryParse(fileId, out var objectId))
            {
                return null;
            }

            var filter = Builders<GridFSFileInfo>.Filter.Eq("_id", objectId);

            if (!_buckets.TryGetValue(collection, out var gridFSBucket))
            {
                throw new ArgumentException($"No GridFS bucket configured for collection: {collection}");
            }
            var fileInfo = await gridFSBucket.Find(filter).FirstOrDefaultAsync(cancellationToken);


            if (fileInfo == null)
            {
                return null;
            }

            var memoryStream = new MemoryStream();
            await gridFSBucket.DownloadToStreamAsync(objectId, memoryStream);

            memoryStream.Position = 0;
            var fileName = fileInfo.Filename;

            return new FileDto
            {
                Stream = memoryStream,
                FileName = fileName,
            };
        }

        public async Task DeleteFileAsync(
            string fileId,
            MongoDbCollection collection = MongoDbCollection.Recruitments,
            CancellationToken cancellationToken = default)
        {
            if (ObjectId.TryParse(fileId, out var objectId))
            {
                if (!_buckets.TryGetValue(collection, out var gridFSBucket))
                {
                    throw new ArgumentException($"No GridFS bucket configured for collection: {collection}");
                }
                await gridFSBucket.DeleteAsync(objectId, cancellationToken);
            }
        }
    }
}
