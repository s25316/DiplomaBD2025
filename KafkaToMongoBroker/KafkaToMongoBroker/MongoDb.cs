// Ignore spelling: Mongo, MongoDb
using MongoDB.Bson;
using MongoDB.Driver;
using System.Text.Json;

namespace KafkaToMongoBroker
{
    public class MongoDb : IDisposable
    {
        // Static  Properties
        public static string ConnectionString { get; private set; }
        public static string Database { get; private set; }

        // Non Static  Properties
        private readonly string _collectionName;
        private readonly MongoClient _client;
        private readonly IMongoCollection<BsonDocument> _collection;
        private bool _disposed = false;


        // Constructor
        static MongoDb()
        {
            ConfigureConnectionString("MongoConnectionString");
            ConfigureDatababse("MongoDatabase");

            Console.WriteLine($"MongDb {nameof(ConnectionString)}: {ConnectionString}");
            Console.WriteLine($"MongDb {nameof(Database)}: {Database}");
        }

        public MongoDb(string collectionName)
        {
            _collectionName = collectionName;
            _client = new MongoClient(ConnectionString);
            var database = _client.GetDatabase(Database);
            _collection = database.GetCollection<BsonDocument>(collectionName);

        }


        // Public Non Static Methods
        public async Task SaveAsync(IEnumerable<string> items)
        {
            var bsonDocuments = new List<BsonDocument>();
            foreach (var item in items)
            {
                if (BsonDocument.TryParse(item, out var bsonDocument))
                {
                    bsonDocuments.Add(bsonDocument);
                }
                else
                {
                    var invalidItem = new
                    {
                        Text = item,
                        Created = DateTime.Now,
                    };
                    var invalidItemString = JsonSerializer.Serialize(invalidItem);
                    bsonDocuments.Add(BsonDocument.Parse(invalidItemString));
                }
            }
            await _collection.InsertManyAsync(bsonDocuments);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            Console.WriteLine($"MongoDB Resources Disposed for Collection: {_collectionName}");
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            if (disposing)
            {
                _client.Dispose();
            }

            _disposed = true;
        }

        ~MongoDb()
        {
            Dispose(false);
        }

        // Private Static Methods
        private static string? GetConfigureProperty(string propertyName)
        {
            return Environment.GetEnvironmentVariable(propertyName);
        }

        private static void ConfigureConnectionString(string propertyName)
        {
            ConnectionString = GetConfigureProperty(propertyName) ??
                throw new KeyNotFoundException("Mongo connection string: Not configured");
        }

        private static void ConfigureDatababse(string propertyName)
        {
            Database = GetConfigureProperty(propertyName) ??
                throw new KeyNotFoundException("Mongo Database: Not configured");
        }
    }
}
