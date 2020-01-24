using Stocks.Ameritrade.Services;
using Stocks.Ameritrade.Services.Interfaces;

namespace Stocks.Ameritrade.Tests.Fixtures
{
    public class AmeritradeFixture
    {
        public IAmeritradeApiService AmeritradeApiService;
        public AmeritradeFixture()
        {
            var apiSettings = AmeritradeSettings.GetSettings();
            AmeritradeApiService = new AmeritradeApiService(apiSettings);
        }
    }
}
