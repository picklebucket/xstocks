using Stocks.Common.Models;
using Stocks.Intrinio.Models.Currency;
using System.Threading.Tasks;

namespace Stocks.Intrinio.Services.Interfaces
{
    public interface IIntrinioForexCurrencyService
    {
        Task<ServiceResponse<CurrencyDataResponse>> GetCurrencyData(CurrencyDataRequest request);
    }
}
