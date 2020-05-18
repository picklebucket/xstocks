using MongoDB.Driver;
using Stocks.Data.Entities;
using Stocks.Data.Repositories.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stocks.Data.Repositories
{
    public class CurrencyDataRepository : BaseRepository<CurrencyData>, ICurrencyDataRepository
    {
        protected override string CollectionName => "CurrencyData";

        public CurrencyDataRepository(StocksContext context) : base(context)
        {

        }

        public async Task<List<CurrencyData>> GetPairData(string pair)
        {
            IMongoCollection<CurrencyData> collection = Context.GetCollection<CurrencyData>(CollectionName);
            var filter = Builders<CurrencyData>.Filter.Eq(x => x.PairCode, pair);
            return await collection.Find(filter).ToListAsync().ConfigureAwait(false);
        }

        public async Task WipeData(string pair)
        {
            IMongoCollection<CurrencyData> collection = Context.GetCollection<CurrencyData>(CollectionName);
            var filter = Builders<CurrencyData>.Filter.Eq(x => x.PairCode, pair);
            await collection.DeleteOneAsync(filter).ConfigureAwait(false);
        }
    }
}
