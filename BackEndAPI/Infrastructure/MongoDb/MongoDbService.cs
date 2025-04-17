// Ignore Spelling: Mongo
using MongoDB.Driver;
using UseCase.MongoDb;
using UseCase.MongoDb.Models.UserActions;

namespace Infrastructure.MongoDb
{
    public class MongoDbService : IMongoDbService
    {
        // Properties
        private static readonly string _userLogs = UseCase.Configuration.MongoCollectionUserLogs;
        private readonly IMongoDatabase _database;


        // Constructor
        public MongoDbService(IMongoDatabase database)
        {
            _database = database;
        }

        public async Task SaveUserAction(UserActionDto action)
        {
            var collection = _database.GetCollection<UserActionDto>(_userLogs);
            await collection.InsertOneAsync(action);
        }
    }
}
