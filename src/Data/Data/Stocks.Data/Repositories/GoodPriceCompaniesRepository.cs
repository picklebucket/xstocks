using MongoDB.Driver;
using Stocks.Data.Entities;
using Stocks.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stocks.Data.Repositories
{
    public class GoodPriceCompaniesRepository : BaseRepository<GoodPriceCompany>, IGoodPriceCompaniesRepository
    {
        protected override string CollectionName => "GoodPriceCompanies";

        public GoodPriceCompaniesRepository(StocksContext context) : base(context)
        {

        }

        public async Task DeleteAsync(string symbol)
        {
            IMongoCollection<GoodPriceCompany> collection = Context.GetCollection<GoodPriceCompany>(CollectionName);
            var filter = Builders<GoodPriceCompany>.Filter.Eq(x => x.Symbol, symbol);
            await collection.DeleteManyAsync(filter).ConfigureAwait(false);
        }

        public async Task<GoodPriceCompany> GetAsync(string symbol)
        {
            IMongoCollection<GoodPriceCompany> collection = Context.GetCollection<GoodPriceCompany>(CollectionName);
            var filter = Builders<GoodPriceCompany>.Filter.Eq(x => x.Symbol, symbol);
            return await collection.Find(filter).FirstOrDefaultAsync().ConfigureAwait(false);
        }

        public async Task<IReadOnlyCollection<GoodPriceCompany>> GetForPeriod(DateTime? start = null, DateTime? end = null)
        {
            IMongoCollection<GoodPriceCompany> collection = Context.GetCollection<GoodPriceCompany>(CollectionName);
            FilterDefinition<GoodPriceCompany> filter = null;
            if (start.HasValue)
            {
                filter = Builders<GoodPriceCompany>.Filter.Gte(x => x.DateTime, start.Value);
            }

            if (end.HasValue)
            {
                filter = filter == null
                    ? Builders<GoodPriceCompany>.Filter.Lte(x => x.DateTime, end.Value)
                    : filter & Builders<GoodPriceCompany>.Filter.Lte(x => x.DateTime, end.Value);
            }

            if (filter != null)
            {
                return await collection.Find(filter).ToListAsync().ConfigureAwait(false);
            }

            return await collection.AsQueryable().ToListAsync().ConfigureAwait(false);
        }
    }
}
