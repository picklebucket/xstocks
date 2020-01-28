using Microsoft.Extensions.Configuration;
using Stocks.Ameritrade.Models;
using Stocks.Ameritrade.Services;
using Stocks.Ameritrade.Services.Interfaces;
using Stocks.Data;
using Stocks.Data.Models;
using Stocks.Data.Repositories;
using Stocks.Data.Repositories.Interfaces;
using Stocks.Pipelines.Models;
using Stocks.Pipelines.Services;
using Stocks.Pipelines.Services.Interfaces;
using Stocks.Services;
using Stocks.Services.Interfaces;

namespace Stocks.Tests.Fixtures
{
    public class StocksFixture
    {
        public ICollectingService CollectingService { get; }
        public IPipelineManagerService PipelineManagerService { get; }
        public StocksFixture()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.json.user", true)
                .Build();


            var ameritradeSection = configuration.GetSection("Ameritrade");
            var apiSettings = new ApiSettings();
            ameritradeSection.Bind(apiSettings);
            IAmeritradeApiService ameritradeApiService = new AmeritradeApiService(apiSettings);

            var connectionStringsSection = configuration.GetSection("ConnectionStrings");
            var connectionStrings = new ConnectionStrings();
            connectionStringsSection.Bind(connectionStrings);
            var stocksContext = new StocksContext(connectionStrings.Stocks);
            IPriceHistoryRepository priceHistoryRepository = new PriceHistoryRepository(stocksContext);

            CollectingService = new CollectingService(ameritradeApiService, priceHistoryRepository);

            var pipelinesSection = configuration.GetSection("Pipelines");
            var pipelineSettings = new PipelineSettings();
            pipelinesSection.Bind(pipelineSettings);
            PipelineManagerService = new PipelineManagerService(pipelineSettings);
        }
    }
}
