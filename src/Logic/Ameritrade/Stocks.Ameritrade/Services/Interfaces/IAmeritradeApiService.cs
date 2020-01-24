using Stocks.Ameritrade.Models;
using Stocks.Ameritrade.Models.PriceHistory;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Ameritrade.Services.Interfaces
{
    public interface IAmeritradeApiService
    {
        Task<ApiResponse<PriceHistoryResponse>> GetPriceHistory(string symbol, PriceHistoryRequest priceHistoryRequest);
    }
}
