using MongoDB.Driver;
using Stocks.Data.Entities;
using Stocks.Data.Repositories.Interfaces;
using System.Threading.Tasks;

namespace Stocks.Data.Repositories
{
    public class NewsDetailsRepository : BaseRepository<NewsDetails>, INewsDetailsRepository
    {
        protected override string CollectionName => "NewsDetailsData";

        public NewsDetailsRepository(StocksContext context) : base(context)
        {

        }

        public async Task Update(NewsDetails news)
        {
            if (news != null)
            {
                IMongoCollection<NewsDetails> collection = Context.GetCollection<NewsDetails>(CollectionName);
                var filter = Builders<NewsDetails>.Filter.Eq(s => s.Id, news.Id);
                var result = await collection.ReplaceOneAsync(filter, news).ConfigureAwait(false);
            }
        }
    }
}
