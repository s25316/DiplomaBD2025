// Ignore Spelling: Mongo
using MongoDB.Bson;
using MongoDB.Driver;
using UseCase.MongoDb;
using UseCase.MongoDb.Enums;
using UseCase.MongoDb.UserLogs;
using UseCase.MongoDb.UserLogs.DTOs;
using UseCase.MongoDb.UserLogs.DTOs.UserAuthorizationDtos;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserAuthorizationEvents;

namespace Infrastructure.MongoDb
{
    public class MongoDbService : IMongoDbService
    {
        // Static Properties
        private static readonly string _idPropertyName = "_id";
        private static readonly string _typeIdPropertyName = nameof(BaseLogMongoDb.TypeId);
        private static readonly string _createdPropertyName = nameof(BaseLogMongoDb.Created);

        private static readonly string _userIdPropertyName = nameof(BaseUserLogMongoDb.UserId);

        private static readonly string _urlSegment2StagePropertyName = nameof(UserAuthorization2StageMongoDb.UrlSegment);
        private static readonly string _code2StagePropertyName = nameof(UserAuthorization2StageMongoDb.Code);
        private static readonly string _isDeactivated2StagePropertyName = nameof(UserAuthorization2StageMongoDb.IsDeactivated);

        // Non Static Properties
        private readonly IMongoCollection<BsonDocument> _userLogsCollection;


        // Constructor
        public MongoDbService(IMongoDatabase database)
        {
            _userLogsCollection = database
                .GetCollection<BsonDocument>(UseCase.Configuration.MongoCollectionUserLogs);

        }


        // Public Methods
        public async Task<UserActivationDataMongoDbDto> GetUserActivationDataAsync(
            Guid userId,
            CancellationToken cancellationToken)
        {

            Console.WriteLine(string.Join(", ", UserActivationDataMongoDbDto.TypeIds));
            var pipeline = PrepareLastUserDataPipeline(userId, UserActivationDataMongoDbDto.TypeIds);
            var logs = await GetUserLogsAsync(pipeline, cancellationToken);
            return (UserActivationDataMongoDbDto)logs.ToList();
        }

        public async Task<UserLoginInDataMongoDbDto> GetUserLoginInDataAsync(
            Guid userId,
            CancellationToken cancellationToken)
        {
            var pipeline = PrepareUserLoginInDataPipeline(userId);
            var logs = await GetUserLogsAsync(pipeline, cancellationToken);
            return (UserLoginInDataMongoDbDto)logs.ToList();
        }

        public async Task<User2StageDataMongoDbDto> GetUser2StageDataAsync(
            Guid userId,
            string urlSegment,
            string code,
            CancellationToken cancellationToken)
        {
            var pipeline = PrepareUser2StageDataPipeline(userId, urlSegment, code);
            var bsonDocument = await GetBsonDocumentAsync(pipeline, cancellationToken);
            if (bsonDocument == null)
            {
                return User2StageDataMongoDbDto.PrepareEmpty();
            }

            var baseLog = BaseLogMongoDb.Map(bsonDocument);
            if (baseLog is not UserAuthorization2StageMongoDb log2Stage)
            {
                throw new Exception();
            }

            await DeactivateUser2StageDataAsync(bsonDocument, cancellationToken);
            return User2StageDataMongoDbDto.PrepareNotEmpty(log2Stage);
        }

        // Private Static Methods
        private static BsonDocument[] PrepareLastUserDataPipeline(
            Guid userId,
            IEnumerable<int> typeIds)
        {
            var userIdMatchStage = new BsonDocument("$match",
                new BsonDocument(
                    _userIdPropertyName,
                    userId.ToString().ToLower()));

            var typeIdMatchStage = new BsonDocument("$match",
                new BsonDocument(_typeIdPropertyName,
                    new BsonDocument("$in",
                        new BsonArray(typeIds))));

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

            return [
                userIdMatchStage,
                typeIdMatchStage,
                sortByCreatedStage,
                groupByTypeIdStage,
                replaceRootStage];
        }

        private static BsonDocument[] PrepareUser2StageDataPipeline(
            Guid userId,
            string urlSegment,
            string code)
        {
            var userIdMatchStage = new BsonDocument("$match",
                    new BsonDocument(
                        _userIdPropertyName,
                        userId.ToString().ToLower()));

            var urlSegmentMatchStage = new BsonDocument("$match",
                new BsonDocument(
                    _urlSegment2StagePropertyName,
                    urlSegment));

            var codeMatchStage = new BsonDocument("$match",
                new BsonDocument(
                    _code2StagePropertyName,
                    code));

            var typeIdMatchStage = new BsonDocument("$match",
                new BsonDocument(
                    _typeIdPropertyName,
                    User2StageDataMongoDbDto.TypeId));

            var isDeactivatedMatchStage = new BsonDocument("$match",
                new BsonDocument(
                    _isDeactivated2StagePropertyName,
                    false));

            var sortByCreatedStage = new BsonDocument("$sort",
                new BsonDocument(
                    _createdPropertyName,
                    -1));

            return [
                userIdMatchStage,
                urlSegmentMatchStage,
                codeMatchStage,
                typeIdMatchStage,
                isDeactivatedMatchStage,
                sortByCreatedStage];
        }

        private static BsonDocument[] PrepareUserLoginInDataPipeline(
            Guid userId)
        {
            // Prepare Ids
            var handStageId = (int)typeof(UserAuthorization2StageMongoDb).GetMongoLog();
            var typeIds = UserLoginInDataMongoDbDto.TypeIds;

            // Prepare pipeline
            var userIdMatchStage = new BsonDocument("$match",
                new BsonDocument(
                    _userIdPropertyName,
                    userId.ToString().ToLower()));

            var typeIdMatchStage = new BsonDocument("$match",
                new BsonDocument("$or", new BsonArray
                {
                    new BsonDocument("$and", new BsonArray
                    {
                        new BsonDocument(_typeIdPropertyName, new BsonDocument("$in", new BsonArray(typeIds))),
                        new BsonDocument(_typeIdPropertyName, new BsonDocument("$ne", handStageId))
                    }),

                    new BsonDocument("$and", new BsonArray
                    {
                        new BsonDocument(_typeIdPropertyName, handStageId),
                        new BsonDocument(_isDeactivated2StagePropertyName, true)
                    })
                }));

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

            return [
                userIdMatchStage,
                typeIdMatchStage,
                sortByCreatedStage,
                groupByTypeIdStage,
                replaceRootStage];
        }

        // Private Non Static Methods
        public async Task<IEnumerable<BsonDocument>> GetBsonDocumentsAsync(
            BsonDocument[] pipeline,
            CancellationToken cancellationToken)
        {
            return await _userLogsCollection
                .Aggregate<BsonDocument>(pipeline)
                .ToListAsync(cancellationToken);
        }
        public async Task<BsonDocument?> GetBsonDocumentAsync(
            BsonDocument[] pipeline,
            CancellationToken cancellationToken)
        {
            return await _userLogsCollection
                .Aggregate<BsonDocument>(pipeline)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<IEnumerable<BaseLogMongoDb>> GetUserLogsAsync(
            BsonDocument[] pipeline,
            CancellationToken cancellationToken)
        {
            var bsonDocuments = await GetBsonDocumentsAsync(pipeline, cancellationToken);
            return bsonDocuments.Select(BaseLogMongoDb.Map);
        }

        public async Task DeactivateUser2StageDataAsync(
            BsonDocument bsonDocument,
            CancellationToken cancellationToken)
        {
            // Deactivate 
            var update = Builders<BsonDocument>.Update
                .Set(_isDeactivated2StagePropertyName, true);
            var filter = Builders<BsonDocument>
                .Filter.Eq(_idPropertyName, bsonDocument[_idPropertyName]);
            var updateResult = await _userLogsCollection
                .UpdateOneAsync(filter, update);
            if (updateResult.ModifiedCount == 0)
            {
                throw new Exception("Have no update");
            }
        }
    }
}
