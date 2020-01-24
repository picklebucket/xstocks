using Stocks.Data.Entities;
using Stocks.Data.Repositories.Interfaces;

namespace Stocks.Data.Repositories
{
    public class PriceHistoryRepository : BaseRepository<PriceHistory>, IPriceHistoryRepository
    {
        protected override string CollectionName => "PriceHistory";

        public PriceHistoryRepository(StocksContext context) : base(context)
        {

        }
    }
}
