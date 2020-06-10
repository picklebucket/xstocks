using Stocks.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Services.Interfaces
{
    public interface IPriceHistoryDataService
    {
        Task CreateAsync(string symbol, bool empty, IEnumerable<CandleItem> candles);
        Task<List<CandleItem>> GetCandlesAsync(string symbol);
        Task<List<CandleItem>> GetCandlesAsync(string symbol, DateTime start, DateTime end);
        Task AddToSymbol(string symbol, IEnumerable<CandleItem> candles);
    }
}
