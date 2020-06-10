using Stocks.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stocks.Data.Repositories.Interfaces
{
    public interface IPriceHistoryRepository : IBaseRepository<PriceHistory>
    {
        Task DeleteAsync(string symbol);
        Task<PriceHistory> GetAsync(string symbol);
        Task AddToSymbol(string symbol, List<Candle> candles);
    }
}
