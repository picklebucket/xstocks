using Intrinio.SDK.Api;
using Stocks.Intrinio.Models;

namespace Stocks.Intrinio.Wrappers
{
    public class IntrinioFundamentalsApi : FundamentalsApi
    {
        public IntrinioFundamentalsApi(IntrinioApiSettings settings) : base(IntrinioConfiguration.Create(settings))
        {

        }
    }
}
