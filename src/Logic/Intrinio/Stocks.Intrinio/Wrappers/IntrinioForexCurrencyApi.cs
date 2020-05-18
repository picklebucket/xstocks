using Intrinio.SDK.Api;
using Stocks.Intrinio.Models;

namespace Stocks.Intrinio.Wrappers
{
    public class IntrinioForexCurrencyApi : ForexApi
    {
        public IntrinioForexCurrencyApi(IntrinioApiSettings settings) : base(IntrinioConfiguration.Create(settings))
        {

        }
    }
}
