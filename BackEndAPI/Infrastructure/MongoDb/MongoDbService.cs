// Ignore Spelling: Mongo
using MongoDB.Driver;
using UseCase.MongoDb;
using UseCase.MongoDb.Models.UserActions;

namespace Infrastructure.MongoDb
{
    public class MongoDbService : IMongoDbService
    {
        // Properties
        private static readonly string _connectionString = UseCase.Configuration.MongoDbConnectionString;
        private static readonly string _database = UseCase.Configuration.MongoDbDatabase;
        private static readonly string _userLogs = UseCase.Configuration.MongoCollectionUserLogs;



        public async Task SaveUserAction(UserActionDto action)
        {
            using (var mongoClient = new MongoClient(_connectionString))
            {
                var database = mongoClient.GetDatabase(_database);
                var collection = database.GetCollection<UserActionDto>(_userLogs);
                await collection.InsertOneAsync(action);
            }
        }
    }
}
