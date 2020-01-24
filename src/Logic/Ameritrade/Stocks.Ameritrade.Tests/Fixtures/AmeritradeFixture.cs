using Microsoft.Extensions.Configuration;
using Stocks.Ameritrade.Models;
using Stocks.Ameritrade.Services;
using Stocks.Ameritrade.Services.Interfaces;

namespace Stocks.Ameritrade.Tests.Fixtures
{
    public class AmeritradeFixture
    {
        public IAmeritradeApiService AmeritradeApiService;
        public AmeritradeFixture()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.json.user", true)
                .Build();
            var apiSettings = new ApiSettings();
            configuration.Bind(apiSettings);

            AmeritradeApiService = new AmeritradeApiService(apiSettings);
        }
    }
}
