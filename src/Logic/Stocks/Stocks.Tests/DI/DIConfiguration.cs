using Microsoft.Extensions.DependencyInjection;
using Stocks.Ameritrade.Models;
using Stocks.Ameritrade.Services;
using Stocks.Ameritrade.Services.Interfaces;
using Stocks.Data;
using Stocks.Data.Repositories;
using Stocks.Data.Repositories.Interfaces;
using Stocks.Pipelines.DI;
using Stocks.Pipelines.Models;
using Stocks.Pipelines.Services;
using Stocks.Pipelines.Services.Interfaces;
using Stocks.Services;
using Stocks.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Tests.DI
{
    static class DIConfiguration
    {
        public static ServiceProvider GetServiceProvider(AmeritradeApiSettings ameritradeApiSettings, StocksContext stocksContext, PipelineSettings pipelineSettings)
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection, ameritradeApiSettings, stocksContext, pipelineSettings);
            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            return serviceProvider;
        }

        private static void ConfigureServices(IServiceCollection serviceCollection, AmeritradeApiSettings ameritradeApiSettings, StocksContext stocksContext, PipelineSettings pipelineSettings)
        {
            serviceCollection.AddSingleton(ameritradeApiSettings);
            serviceCollection.AddTransient<IAmeritradeApiService, AmeritradeApiService>();
            serviceCollection.AddSingleton(stocksContext);
            serviceCollection.AddTransient<IPriceHistoryRepository, PriceHistoryRepository>();
            serviceCollection.AddTransient<ICollectingService, CollectingService>();
            serviceCollection.AddSingleton(pipelineSettings);
            serviceCollection.AddTransient<IPipelineManagerService, PipelineManagerService>();
            serviceCollection.AddPipelineProcessors(pipelineSettings);
        }
    }
}
