using Stocks.Ameritrade.Models;
using Stocks.Ameritrade.Models.PriceHistory;
using Stocks.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Ameritrade.Services.Interfaces
{
    public interface IAmeritradeApiService
    {
        Task<ServiceResponse<PriceHistoryResponse>> GetPriceHistoryAsync(string symbol, AmeritradePriceHistoryRequest priceHistoryRequest);
    }
}
