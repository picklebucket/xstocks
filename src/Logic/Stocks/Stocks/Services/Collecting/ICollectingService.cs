using Stocks.Common.Models;
using Stocks.Models.Api.PriceHistory;
using System.Threading.Tasks;

namespace Stocks.Services.Collecting
{
    public interface ICollectingService
    {
        Task<ServiceResponse> CollectDataAsync(PriceHistoryRequest priceHistoryRequest);
    }
}
