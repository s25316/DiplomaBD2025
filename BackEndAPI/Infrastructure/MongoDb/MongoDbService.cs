// Ignore Spelling: Mongo
using MongoDB.Bson;
using MongoDB.Driver;
using UseCase.MongoDb;
using UseCase.MongoDb.Enums;
using UseCase.MongoDb.UserLogs;
using UseCase.MongoDb.UserLogs.DTOs;
using UseCase.MongoDb.UserLogs.Models.UserEvents.AuthenticationEvents;

namespace Infrastructure.MongoDb
{
    public class MongoDbService : IMongoDbService
    {
        // Properties
        private readonly IMongoDatabase _database;

        private static readonly string _userLogs = UseCase.Configuration.MongoCollectionUserLogs;

        private static readonly string _typeIdPropertyName = nameof(BaseLogMongoDb.TypeId);
        private static readonly string _createdPropertyName = nameof(BaseLogMongoDb.Created);
        private static readonly string _userIdPropertyName = nameof(BaseUserLogMongoDb.UserId);

        private static readonly List<int> _activationDtoIds = new()
            {
                (int)MongoLogs.UserProfileCreated,
                (int)MongoLogs.UserProfileActivated
            };
        private static readonly List<int> _loginInDtoIds = new()
            {
                (int)MongoLogs.UserProfileActivated,
                (int)MongoLogs.UserAuthorization2Stage,

                (int)MongoLogs.UserProfileRemoved,
                (int)MongoLogs.UserProfileRestored,

                (int)MongoLogs.UserProfileBlocked,
                (int)MongoLogs.UserProfileUnBlocked,
            };


        // Constructor
        public MongoDbService(IMongoDatabase database)
        {
            _database = database;

        }


        // Methods
        public async Task<UserActivationMongoDbDto> GetUserProfileActivationData(
            Guid userId,
            CancellationToken cancellationToken)
        {
            var logs = await GetLastLogsAsync(
                userId,
                _activationDtoIds,
                cancellationToken);
            return (UserActivationMongoDbDto)logs;
        }

        public async Task<UserLoginInMongoDbDto> GetLoginInDataAsync(
            Guid userId,
            CancellationToken cancellationToken)
        {
            var logs = await GetLastLogsAsync(
                userId,
                _loginInDtoIds,
                cancellationToken);
            return (UserLoginInMongoDbDto)logs;
        }

        public async Task<UserAuthorization2StageMongoDbDto> Get2StageAuthorizationAsync(
            Guid userId,
            string urlSegment,
            string code,
            CancellationToken cancellationToken)
        {
            var urlSegmentPropertyName = nameof(UserAuthorization2StageMongoDb.UrlSegment);
            var codePropertyName = nameof(UserAuthorization2StageMongoDb.Code);


            var collection = _database.GetCollection<BsonDocument>(_userLogs);

            var userIdMatchStage = new BsonDocument("$match",
                new BsonDocument(
                    _userIdPropertyName,
                    userId.ToString().ToLower()));

            var urlSegmentMatchStage = new BsonDocument("$match",
                new BsonDocument(
                    urlSegmentPropertyName,
                    urlSegment));

            var codeMatchStage = new BsonDocument("$match",
                new BsonDocument(
                    codePropertyName,
                    code));

            var typeIdMatchStage = new BsonDocument("$match",
                new BsonDocument(
                    _typeIdPropertyName,
                    (int)MongoLogs.UserAuthorization2Stage));

            var sortByCreatedStage = new BsonDocument("$sort",
                new BsonDocument(
                    _createdPropertyName,
                    -1));

            var pipeline = new[]
            {
                userIdMatchStage,
                urlSegmentMatchStage,
                codeMatchStage,
                typeIdMatchStage,
                sortByCreatedStage
            };

            var bsonDocument = await collection
                .Aggregate<BsonDocument>(pipeline, cancellationToken: cancellationToken)
                .FirstOrDefaultAsync(cancellationToken);

            if (bsonDocument == null)
            {
                return new UserAuthorization2StageMongoDbDto
                {
                    Item = null,
                };
            }

            var baseLog = BaseLogMongoDb.Map(bsonDocument);
            if (baseLog is UserAuthorization2StageMongoDb log)
            {
                return new UserAuthorization2StageMongoDbDto
                {
                    Item = log,
                };
            }
            else
            {
                throw new NotImplementedException("Something Changed");
            }
        }

        // Private Methods
        private async Task<List<BaseLogMongoDb>> GetLastLogsAsync(
            Guid userId,
            List<int> documentIds,
            CancellationToken cancellationToken)
        {
            var collection = _database.GetCollection<BsonDocument>(_userLogs);

            var userIdMatchStage = new BsonDocument("$match",
                new BsonDocument(
                    _userIdPropertyName,
                    userId.ToString().ToLower()));

            var typeIdMatchStage = new BsonDocument("$match",
                new BsonDocument(_typeIdPropertyName,
                    new BsonDocument("$in",
                    new BsonArray(documentIds))));

            var sortByCreatedStage = new BsonDocument("$sort",
                new BsonDocument(
                    _createdPropertyName,
                    -1));

            var groupByTypeIdStage = new BsonDocument("$group",
                new BsonDocument
                {
                    { "_id", $"${_typeIdPropertyName}" },
                    { "lastDocument", new BsonDocument("$first", "$$ROOT") }
                });

            var replaceRootStage = new BsonDocument("$replaceRoot",
                new BsonDocument("newRoot", "$lastDocument"));

            var pipeline = new[]
            {
                userIdMatchStage,
                typeIdMatchStage,
                sortByCreatedStage,
                groupByTypeIdStage,
                replaceRootStage
            };

            var bsonDocuments = await collection
                .Aggregate<BsonDocument>(pipeline, cancellationToken: cancellationToken)
                .ToListAsync(cancellationToken);

            return bsonDocuments
                .Select(BaseLogMongoDb.Map)
                .ToList();
        }
    }
}
