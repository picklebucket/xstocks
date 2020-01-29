using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stocks.Ameritrade.Models;
using Stocks.Data;
using Stocks.Data.Models;
using Stocks.Data.Repositories.Interfaces;
using Stocks.Pipelines.Models;
using Stocks.Pipelines.Services.Interfaces;
using Stocks.Services.Interfaces;
using Stocks.Tests.DI;

namespace Stocks.Tests.Fixtures
{
    public class StocksFixture
    {
        public ICollectingService CollectingService { get; }
        public IPipelineManagerService PipelineManagerService { get; }
        public IPriceHistoryRepository PriceHistoryRepository { get; }

        public StocksFixture()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.json.user", true)
                .Build();            

            var ameritradeSection = configuration.GetSection("Ameritrade");
            var ameritradeApiSettings = new AmeritradeApiSettings();
            ameritradeSection.Bind(ameritradeApiSettings);
            
            var connectionStringsSection = configuration.GetSection("ConnectionStrings");
            var connectionStrings = new ConnectionStrings();
            connectionStringsSection.Bind(connectionStrings);
            var stocksContext = new StocksContext(connectionStrings.Stocks);

            var pipelinesSection = configuration.GetSection("Pipelines");
            var pipelineSettings = new PipelineSettings();
            pipelinesSection.Bind(pipelineSettings);

            var serviceProvider = DIConfiguration.GetServiceProvider(ameritradeApiSettings, stocksContext, pipelineSettings);
            PipelineManagerService = serviceProvider.GetService<IPipelineManagerService>();
            CollectingService = serviceProvider.GetService<ICollectingService>();
            PriceHistoryRepository = serviceProvider.GetService<IPriceHistoryRepository>();
        }
    }
}
