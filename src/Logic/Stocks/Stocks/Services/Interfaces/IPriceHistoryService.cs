using Stocks.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Services.Interfaces
{
    public interface IPriceHistoryService
    {
        Task<List<CandleItem>> GetCollectedCandlesAsync(string symbol);
    }
}
