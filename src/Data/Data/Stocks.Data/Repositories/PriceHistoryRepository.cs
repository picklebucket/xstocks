using MongoDB.Driver;
using Stocks.Common.Extensions;
using Stocks.Data.Entities;
using Stocks.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
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
            await collection.DeleteManyAsync(filter).ConfigureAwait(false);
        }

        public async Task<PriceHistory> GetAsync(string symbol)
        {
            IMongoCollection<PriceHistory> collection = Context.GetCollection<PriceHistory>(CollectionName);
            var filter = Builders<PriceHistory>.Filter.Eq(x => x.Symbol, symbol);
            return await collection.Find(filter).FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task AddToSymbol(string symbol, List<Candle> candles)
        {
            IMongoCollection<PriceHistory> collection = Context.GetCollection<PriceHistory>(CollectionName);
            var filter = Builders<PriceHistory>.Filter.Eq(x => x.Symbol, symbol);

            foreach (var candle in candles)
            {
                await collection.UpdateOneAsync(filter,
                        Builders<PriceHistory>.Update.AddToSet(u => u.Candles, candle)).ConfigureAwait(false);
            }
        }
    }
}
