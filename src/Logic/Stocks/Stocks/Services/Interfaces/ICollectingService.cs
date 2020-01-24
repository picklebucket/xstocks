using Stocks.Ameritrade.Models.PriceHistory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Services.Interfaces
{
    public interface ICollectingService
    {
        Task CollectDataAsync(string symbol, PriceHistoryRequest priceHistoryRequest);
    }
}
