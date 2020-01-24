using MongoDB.Driver;

namespace Stocks.Data
{
    public class StocksContext
    {
        private readonly MongoClient _mongoClient;
        private readonly IMongoDatabase _database;

        public StocksContext(string connectionString)
        {
            string databaseName = MongoUrl.Create(connectionString).DatabaseName;
            _mongoClient = new MongoClient(connectionString); 
            _database = _mongoClient.GetDatabase(databaseName);
        }

        public IMongoCollection<T> GetCollection<T>(string name)
        {
            return _database.GetCollection<T>(name);
        }
    }
}
