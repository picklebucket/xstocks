using MongoDB.Driver;
using Stocks.Data.Entities;
using Stocks.Data.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Stocks.Data.Repositories
{
    public class PriceHistoryRepository : BaseRepository<PriceHistory>, IPriceHistoryRepository
    {
        protected override string CollectionName => "PriceHistory";

        public PriceHistoryRepository(StocksContext context) : base(context)
        {

        }

        public async Task DeleteAsync(string symbol)
        {
            IMongoCollection<PriceHistory> collection = Context.GetCollection<PriceHistory>(CollectionName);
            var filter = Builders<PriceHistory>.Filter.Eq(x => x.Symbol, symbol);
            await collection.DeleteOneAsync(filter);
        }

        public async Task<PriceHistory> GetAsync(string symbol)
        {
            IMongoCollection<PriceHistory> collection = Context.GetCollection<PriceHistory>(CollectionName);
            var filter = Builders<PriceHistory>.Filter.Eq(x => x.Symbol, symbol);
            return await collection.Find(filter).FirstOrDefaultAsync();
        }
    }
}
