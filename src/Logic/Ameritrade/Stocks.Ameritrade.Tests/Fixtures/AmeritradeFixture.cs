using Microsoft.Extensions.Configuration;
using Stocks.Ameritrade.Models;
using Stocks.Ameritrade.Services;
using Stocks.Ameritrade.Services.Interfaces;

namespace Stocks.Ameritrade.Tests.Fixtures
{
    public class AmeritradeFixture
    {
        public IAmeritradeApiService AmeritradeApiService { get; }
        public AmeritradeFixture()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.json.user", true)
                .Build();
            var ameritradeSection = configuration.GetSection("Ameritrade");
            var apiSettings = new AmeritradeApiSettings();
            ameritradeSection.Bind(apiSettings);

            AmeritradeApiService = new AmeritradeApiService(apiSettings);
        }
    }
}
