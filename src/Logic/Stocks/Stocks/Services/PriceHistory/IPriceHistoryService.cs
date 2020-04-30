using Stocks.Common.Models;
using Stocks.Models.Api.PriceHistory;
using System.Threading.Tasks;

namespace Stocks.Services.Collecting
{
    public interface IPriceHistoryService
    {
        Task<ServiceResponse<PriceHistoryResponse>> GetPriceHistoryAsync(PriceHistoryRequest priceHistoryRequest);
    }
}
