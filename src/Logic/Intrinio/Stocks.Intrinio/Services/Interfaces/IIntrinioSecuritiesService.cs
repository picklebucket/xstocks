using Intrinio.SDK.Model;
using Stocks.Common.Models;
using Stocks.Intrinio.Models.Ichimoku;
using Stocks.Intrinio.Models.PriceHistory;
using System;
using System.Threading.Tasks;

namespace Stocks.Intrinio.Services.Interfaces
{
    public interface IIntrinioSecuritiesService
    {
        Task<ServiceResponse<IntrinioPriceHistoryResponse>> GetStockPricesAsync(IntrinioPriceHistoryRequest priceHistoryRequest);
        Task<ServiceResponse<IntrinioIchimokuResponse>> GetIchimokuAsync(IntrinioIchimokuRequest ichimokuRequest);
    }
}
