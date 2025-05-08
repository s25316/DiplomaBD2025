// Ignore Spelling: Mongo, Bson, Jwt, Middleware
using Infrastructure.Exceptions;
using MongoDB.Bson;
using MongoDB.Driver;
using UseCase.MongoDb;
using UseCase.MongoDb.Enums;
using UseCase.MongoDb.UserLogs;
using UseCase.MongoDb.UserLogs.DTOs;
using UseCase.MongoDb.UserLogs.DTOs.UserAuthorizationDtos;
using UseCase.MongoDb.UserLogs.DTOs.UserProfileDtos;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserAuthorizationEvents;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.RemoveEvents;
using UseCase.MongoDb.UserLogs.Models.UserEvents.UserProfileEvents.ResetPasswordEvents;

namespace Infrastructure.MongoDb
{
    public class MongoDbService : IMongoDbService
    {
        // Static Properties
        private static readonly string _idPropertyName = "_id";
        private static readonly string _typeIdPropertyName = nameof(BaseLogMongoDb.TypeId);
        private static readonly string _createdPropertyName = nameof(BaseLogMongoDb.Created);
        // BaseUserLogMongoDb
        private static readonly string _userIdPropertyName = nameof(BaseUserLogMongoDb.UserId);
        // UserAuthorization2StageMongoDb
        private static readonly string _urlSegment2StagePropertyName = nameof(UserAuthorization2StageMongoDb.UrlSegment);
        private static readonly string _code2StagePropertyName = nameof(UserAuthorization2StageMongoDb.Code);
        private static readonly string _isDeactivated2StagePropertyName = nameof(UserAuthorization2StageMongoDb.IsDeactivated);
        // UserAuthorizationLogOutMongoDb
        private static readonly string _jwtAuthorizationPropertyName = nameof(UserAuthorizationLogOutMongoDb.Jwt);
        private static readonly string _refreshTokenAuthorizationPropertyName = nameof(UserAuthorizationLogOutMongoDb.RefreshToken);
        // UserAuthorizationRefreshTokenMongoDb
        private static readonly string _isDeactivatedAuthorizationPropertyName = nameof(UserAuthorizationRefreshTokenMongoDb.IsDeactivated);
        // UserProfileInitiatedResetPasswordMongoDb
        private static readonly string _urlSegmentInitResetPasswordPropertyName = nameof(UserProfileInitiatedResetPasswordMongoDb.UrlSegment);
        private static readonly string _isDeactivatedInitResetPasswordPropertyName = nameof(UserProfileInitiatedResetPasswordMongoDb.IsDeactivated);
        // UserProfileRemovedMongoDb
        private static readonly string _urlSegmentProfileRemoved = nameof(UserProfileRemovedMongoDb.UrlSegment);
        private static readonly string _isDeactivatedProfileRemoved = nameof(UserProfileRemovedMongoDb.IsDeactivated);

        private static readonly IEnumerable<int> _typeIdsWithJwt = [
                (int)typeof(UserAuthorizationLoginInMongoDb).GetMongoLog(),
                (int)typeof(UserAuthorizationRefreshTokenMongoDb).GetMongoLog()
            ];
        // GetUserActivationDataAsync
        private static readonly IEnumerable<int> _typeIdsUserActivation = UserActivationDataMongoDbDto.TypeIds;
        // PrepareUserLoginInDataPipeline
        private static readonly IEnumerable<int> _typeIdsUserLoginIn = UserLoginInMongoDbDto.TypeIds;
        private static readonly int _handStageId = (int)typeof(UserAuthorization2StageMongoDb).GetMongoLog();
        // GetUserRefreshTokenDataAsync
        private static readonly IEnumerable<int> _typeIdsUserRefreshToken = UserRefreshTokenMongoDbDto.TypeIds;
        // PrepareUserLogOutDataPipeline
        private static readonly IEnumerable<int> _typeIdsUserLogOut = UserLogOutMongoDbDto.TypeIds;
        // GetUserMiddlewareDataAsync
        private static readonly IEnumerable<int> _typeIdsUserMiddleware = UserMiddlewareMongoDbDto.TypeIds;
        // PrepareUserResetPasswordInitiationPipeline
        private static readonly int _typeIdResetPasswordInitiation = UserResetPasswordInitiationMongoDbDto.TypeId;
        // PrepareUserProfileRemovedPipeline
        private static readonly int _typeIdUserRemoved = UserRemovedMongoDbDto.TypeId;

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
            var pipeline = PrepareLastUserDataPipeline(userId, _typeIdsUserActivation);
            var logs = await GetUserLogsAsync(pipeline, cancellationToken);
            return (UserActivationDataMongoDbDto)logs.ToList();
        }

        public async Task<UserLoginInMongoDbDto> GetUserLoginInDataAsync(
            Guid userId,
            CancellationToken cancellationToken)
        {
            var pipeline = PrepareUserLoginInDataPipeline(userId);
            var logs = await GetUserLogsAsync(pipeline, cancellationToken);
            return (UserLoginInMongoDbDto)logs.ToList();
        }

        public async Task<User2StageMongoDbDto> GetUser2StageDataAsync(
            Guid userId,
            string urlSegment,
            string code,
            CancellationToken cancellationToken)
        {
            var pipeline = PrepareUser2StageDataPipeline(userId, urlSegment, code);
            var bsonDocument = await GetBsonDocumentAsync(pipeline, cancellationToken);
            if (bsonDocument == null)
            {
                return User2StageMongoDbDto.PrepareEmpty();
            }

            var baseLog = BaseLogMongoDb.Map(bsonDocument);
            if (baseLog is not UserAuthorization2StageMongoDb log2Stage)
            {
                throw new InfrastructureLayerException("Something Changed in mapping");
            }

            await DeactivateUser2StageDataAsync(bsonDocument, cancellationToken);
            return User2StageMongoDbDto.PrepareNotEmpty(log2Stage);
        }

        public async Task<UserRefreshTokenMongoDbDto> GetUserRefreshTokenDataAsync(
            Guid userId,
            string jwt,
            string refreshToken,
            CancellationToken cancellationToken)
        {
            var pipeline = PrepareRefreshTokenDataPipeline(userId, jwt, refreshToken, _typeIdsUserRefreshToken);
            var bsonDocuments = await GetBsonDocumentsAsync(pipeline, cancellationToken);

            await DeactivateAuthorizationDataAsync(bsonDocuments, cancellationToken);
            return (UserRefreshTokenMongoDbDto)bsonDocuments
                .Select(BaseLogMongoDb.Map)
                .ToList();
        }

        public async Task<UserLogOutMongoDbDto> GetUserLogOutDataAsync(
            Guid userId,
            string jwt,
            CancellationToken cancellationToken)
        {
            var pipeline = PrepareUserLogOutDataPipeline(userId, jwt);
            var bsonDocuments = await GetBsonDocumentsAsync(pipeline, cancellationToken);

            await DeactivateAuthorizationDataAsync(bsonDocuments, cancellationToken);
            return (UserLogOutMongoDbDto)bsonDocuments
                .Select(BaseLogMongoDb.Map)
                .ToList();
        }

        public async Task<UserMiddlewareMongoDbDto> GetUserMiddlewareDataAsync(
            Guid userId,
            string jwt,
            CancellationToken cancellationToken)
        {
            var pipeline = PrepareIdsWithJwtPipeline(userId, jwt, _typeIdsUserMiddleware);
            var baseLogs = await GetUserLogsAsync(pipeline, cancellationToken);
            return (UserMiddlewareMongoDbDto)baseLogs.ToList();
        }


        public async Task<UserResetPasswordInitiationMongoDbDto> GeUserResetPasswordInitiationAsync(
            Guid userId,
            string urlSegment,
            CancellationToken cancellationToken)
        {
            var pipeline = PrepareUserResetPasswordInitiationPipeline(userId, urlSegment);
            var bsonDocument = await GetBsonDocumentAsync(pipeline, cancellationToken);
            if (bsonDocument == null)
            {
                return new UserResetPasswordInitiationMongoDbDto { };
            }

            await DeactivateResetPasswordInitiationAsync(bsonDocument, cancellationToken);
            return UserResetPasswordInitiationMongoDbDto.Prepare(bsonDocument);
        }

        public async Task<UserRemovedMongoDbDto> GeUserRemovedAsync(
            Guid userId,
            string urlSegment,
            CancellationToken cancellationToken)
        {
            var pipeline = PrepareUserProfileRemovedPipeline(userId, urlSegment);
            var bsonDocument = await GetBsonDocumentAsync(pipeline, cancellationToken);
            if (bsonDocument == null)
            {
                return new UserRemovedMongoDbDto { };
            }
            await DeactivateProfileRemovedAsync(bsonDocument, cancellationToken);
            return UserRemovedMongoDbDto.Prepare(bsonDocument);
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
                    User2StageMongoDbDto.TypeId));

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
                        new BsonDocument(_typeIdPropertyName,
                            new BsonDocument("$in",
                                new BsonArray(_typeIdsUserLoginIn))),
                        new BsonDocument(_typeIdPropertyName,
                            new BsonDocument("$ne", _handStageId))
                    }),

                    new BsonDocument("$and", new BsonArray
                    {
                        new BsonDocument(_typeIdPropertyName, _handStageId),
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

        private static BsonDocument[] PrepareRefreshTokenDataPipeline(
           Guid userId,
           string jwt,
           string refreshToken,
           IEnumerable<int> typeIds)
        {
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
                        new BsonDocument(_typeIdPropertyName,
                            new BsonDocument("$in",
                                new BsonArray(typeIds))),
                        new BsonDocument(_typeIdPropertyName,
                            new BsonDocument("$nin",
                                new BsonArray(_typeIdsWithJwt)))
                    }),

                    new BsonDocument("$and", new BsonArray
                    {
                        new BsonDocument(_typeIdPropertyName,
                            new BsonDocument("$in",
                                new BsonArray(_typeIdsWithJwt))),
                        new BsonDocument(_jwtAuthorizationPropertyName, jwt),
                        new BsonDocument(_refreshTokenAuthorizationPropertyName, refreshToken),
                        new BsonDocument(_isDeactivatedAuthorizationPropertyName, false)
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

        private static BsonDocument[] PrepareIdsWithJwtPipeline(
           Guid userId,
           string jwt,
           IEnumerable<int> typeIds)
        {
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
                        new BsonDocument(_typeIdPropertyName,
                            new BsonDocument("$in",
                                new BsonArray(typeIds))),
                        new BsonDocument(_typeIdPropertyName,
                            new BsonDocument("$nin",
                                new BsonArray(_typeIdsWithJwt)))
                    }),

                    new BsonDocument("$and", new BsonArray
                    {
                        new BsonDocument(_typeIdPropertyName,
                            new BsonDocument("$in",
                                new BsonArray(_typeIdsWithJwt))),
                        new BsonDocument(_jwtAuthorizationPropertyName, jwt),
                        new BsonDocument(_isDeactivatedAuthorizationPropertyName, false)
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

        private static BsonDocument[] PrepareUserLogOutDataPipeline(
           Guid userId,
           string jwt)
        {
            var userIdMatchStage = new BsonDocument("$match",
                new BsonDocument(
                    _userIdPropertyName,
                    userId.ToString().ToLower()));

            var jwtMatchStage = new BsonDocument("$match",
                new BsonDocument(
                    _jwtAuthorizationPropertyName,
                    jwt));

            var typeIdMatchStage = new BsonDocument("$match",
                new BsonDocument(_typeIdPropertyName,
                    new BsonDocument("$in",
                        new BsonArray(_typeIdsUserLogOut))));

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
                jwtMatchStage,
                typeIdMatchStage,
                sortByCreatedStage,
                groupByTypeIdStage,
                replaceRootStage];
        }

        private static BsonDocument[] PrepareUserResetPasswordInitiationPipeline(
            Guid userId,
            string urlSegment)
        {
            var typeIdMatchStage = new BsonDocument("$match",
                new BsonDocument(
                    _typeIdPropertyName,
                    _typeIdResetPasswordInitiation));

            var userIdMatchStage = new BsonDocument("$match",
                new BsonDocument(
                    _userIdPropertyName,
                    userId.ToString().ToLower()));

            var urlSegmentMatchStage = new BsonDocument("$match",
                new BsonDocument(
                    _urlSegmentInitResetPasswordPropertyName,
                    urlSegment));

            var isDeactivatedMatchStage = new BsonDocument("$match",
                new BsonDocument(
                    _isDeactivatedInitResetPasswordPropertyName,
                    false));

            var sortByCreatedStage = new BsonDocument("$sort",
                new BsonDocument(
                    _createdPropertyName,
                    -1));

            return [
                typeIdMatchStage,
                userIdMatchStage,
                urlSegmentMatchStage,
                isDeactivatedMatchStage,
                sortByCreatedStage
            ];
        }

        private static BsonDocument[] PrepareUserProfileRemovedPipeline(
            Guid userId,
            string urlSegment)
        {
            var typeIdMatchStage = new BsonDocument("$match",
                new BsonDocument(
                    _typeIdPropertyName,
                    _typeIdUserRemoved));

            var userIdMatchStage = new BsonDocument("$match",
                new BsonDocument(
                    _userIdPropertyName,
                    userId.ToString().ToLower()));

            var urlSegmentMatchStage = new BsonDocument("$match",
                new BsonDocument(
                    _urlSegmentProfileRemoved,
                    urlSegment));

            var isDeactivatedMatchStage = new BsonDocument("$match",
                new BsonDocument(
                    _isDeactivatedProfileRemoved,
                    false));

            var sortByCreatedStage = new BsonDocument("$sort",
                new BsonDocument(
                    _createdPropertyName,
                    -1));

            return [
                typeIdMatchStage,
                userIdMatchStage,
                urlSegmentMatchStage,
                isDeactivatedMatchStage,
                sortByCreatedStage
            ];
        }
        // Private Non Static Methods
        private async Task<IEnumerable<BsonDocument>> GetBsonDocumentsAsync(
            BsonDocument[] pipeline,
            CancellationToken cancellationToken)
        {
            return await _userLogsCollection
                .Aggregate<BsonDocument>(pipeline)
                .ToListAsync(cancellationToken);
        }

        private async Task<BsonDocument?> GetBsonDocumentAsync(
            BsonDocument[] pipeline,
            CancellationToken cancellationToken)
        {
            return await _userLogsCollection
                .Aggregate<BsonDocument>(pipeline)
                .FirstOrDefaultAsync(cancellationToken);
        }

        private async Task<IEnumerable<BaseLogMongoDb>> GetUserLogsAsync(
            BsonDocument[] pipeline,
            CancellationToken cancellationToken)
        {
            var bsonDocuments = await GetBsonDocumentsAsync(pipeline, cancellationToken);
            return bsonDocuments.Select(BaseLogMongoDb.Map);
        }

        private async Task DeactivateUser2StageDataAsync(
            BsonDocument bsonDocument,
            CancellationToken cancellationToken)
        {
            // Deactivate 
            var update = Builders<BsonDocument>.Update
                .Set(_isDeactivated2StagePropertyName, true);
            var filter = Builders<BsonDocument>
                .Filter.Eq(_idPropertyName, bsonDocument[_idPropertyName]);
            var updateResult = await _userLogsCollection
                .UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
            if (updateResult.ModifiedCount == 0)
            {
                throw new InfrastructureLayerException("Have no update");
            }
        }

        private async Task DeactivateAuthorizationDataAsync(
          IEnumerable<BsonDocument> bsonDocuments,
          CancellationToken cancellationToken)
        {
            foreach (var bsonDocument in bsonDocuments)
            {
                var typeId = (int)bsonDocument[_typeIdPropertyName];
                if (_typeIdsWithJwt.Contains(typeId))
                {
                    var isDeactivated = (bool)bsonDocument[_isDeactivatedAuthorizationPropertyName];
                    if (isDeactivated)
                    {
                        continue;
                    }
                    // For rule should be 1
                    var update = Builders<BsonDocument>.Update
                        .Set(_isDeactivatedAuthorizationPropertyName, true);
                    var filter = Builders<BsonDocument>
                        .Filter.Eq(_idPropertyName, bsonDocument[_idPropertyName]);
                    var updateResult = await _userLogsCollection
                        .UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
                    if (updateResult.ModifiedCount == 0)
                    {
                        throw new InfrastructureLayerException("Have no update");
                    }
                }
            }
        }


        private async Task DeactivateResetPasswordInitiationAsync(
          BsonDocument bsonDocument,
          CancellationToken cancellationToken)
        {
            var typeId = (int)bsonDocument[_typeIdPropertyName];
            var isDeactivated = (bool)bsonDocument[_isDeactivatedInitResetPasswordPropertyName];

            if (typeId == _typeIdResetPasswordInitiation && !isDeactivated)
            {
                // For rule should be 1
                var update = Builders<BsonDocument>.Update
                    .Set(_isDeactivatedInitResetPasswordPropertyName, true);
                var filter = Builders<BsonDocument>
                    .Filter.Eq(_idPropertyName, bsonDocument[_idPropertyName]);
                var updateResult = await _userLogsCollection
                    .UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
                if (updateResult.ModifiedCount == 0)
                {
                    throw new InfrastructureLayerException("Have no update");
                }
            }
        }


        private async Task DeactivateProfileRemovedAsync(
          BsonDocument bsonDocument,
          CancellationToken cancellationToken)
        {
            var typeId = (int)bsonDocument[_typeIdPropertyName];
            var isDeactivated = (bool)bsonDocument[_isDeactivatedProfileRemoved];

            if (typeId == _typeIdUserRemoved && !isDeactivated)
            {
                // For rule should be 1
                var update = Builders<BsonDocument>.Update
                    .Set(_isDeactivatedInitResetPasswordPropertyName, true);
                var filter = Builders<BsonDocument>
                    .Filter.Eq(_idPropertyName, bsonDocument[_idPropertyName]);
                var updateResult = await _userLogsCollection
                    .UpdateOneAsync(filter, update, cancellationToken: cancellationToken);
                if (updateResult.ModifiedCount == 0)
                {
                    throw new InfrastructureLayerException("Have no update");
                }
            }
        }
    }
}
