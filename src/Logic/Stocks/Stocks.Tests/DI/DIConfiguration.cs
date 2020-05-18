using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stocks.Ameritrade.Models;
using Stocks.Ameritrade.Services;
using Stocks.Ameritrade.Services.Interfaces;
using Stocks.Data;
using Stocks.Data.Models;
using Stocks.Data.Repositories;
using Stocks.Data.Repositories.Interfaces;
using Stocks.Helpers;
using Stocks.Intrinio.Models;
using Stocks.Intrinio.Services;
using Stocks.Intrinio.Services.Interfaces;
using Stocks.Intrinio.Wrappers;
using Stocks.Pipelines.DI;
using Stocks.Pipelines.Models;
using Stocks.Pipelines.Services;
using Stocks.Pipelines.Services.Interfaces;
using Stocks.Services;
using Stocks.Services.Collecting;
using Stocks.Services.Ichimoku;
using Stocks.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Tests.DI
{
    static class DIConfiguration
    {
        public static ServiceProvider GetServiceProvider()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            return serviceProvider;
        }

        private static void ConfigureServices(IServiceCollection serviceCollection)
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.json.user", true)
                .Build();

            var ameritradeSection = configuration.GetSection("Ameritrade");
            var ameritradeApiSettings = new AmeritradeApiSettings();
            ameritradeSection.Bind(ameritradeApiSettings);
            serviceCollection.AddSingleton(ameritradeApiSettings);

            var intrinioSection = configuration.GetSection("Intrinio");
            var intrinioApiSettings = new IntrinioApiSettings();
            intrinioSection.Bind(intrinioApiSettings);
            serviceCollection.AddSingleton(intrinioApiSettings);
            serviceCollection.AddSingleton<IntrinioSecurityApi>();
            serviceCollection.AddSingleton<IntrinioFundamentalsApi>();
            serviceCollection.AddSingleton<IntrinioForexCurrencyApi>();

            var connectionStringsSection = configuration.GetSection("ConnectionStrings");
            var connectionStrings = new ConnectionStrings();
            connectionStringsSection.Bind(connectionStrings);
            var stocksContext = new StocksContext(connectionStrings.Stocks);
            serviceCollection.AddSingleton(stocksContext);

            var pipelinesSection = configuration.GetSection("Pipelines");
            var pipelineSettings = new PipelineSettings();
            pipelinesSection.Bind(pipelineSettings);
            serviceCollection.AddSingleton(pipelineSettings);

            serviceCollection.AddTransient<IAmeritradeApiService, AmeritradeApiService>();
            serviceCollection.AddTransient<IIntrinioSecuritiesService, IntrinioSecuritiesService>();
            serviceCollection.AddTransient<IIntrinioFundementalsService, IntrinioFundementalsService>();
            serviceCollection.AddTransient<IPriceHistoryRepository, PriceHistoryRepository>();
            serviceCollection.AddTransient<IPriceHistoryService, IntrinioPriceHistoryService>();
            serviceCollection.AddTransient<IReportService, ReportService>();
            serviceCollection.AddTransient<IIchimokuService, IntrinioIchimokuService>();
            serviceCollection.AddTransient<IPriceHistoryDataService, PriceHistoryDataService>();
            serviceCollection.AddTransient<IGoodPriceCompaniesRepository, GoodPriceCompaniesRepository>();
            serviceCollection.AddTransient<ICalculateIIntrinsicValueService, CalculateIIntrinsicValueService>();
            serviceCollection.AddTransient<ISymbolAnalyzerService, SymbolAnalyzerService>();
            serviceCollection.AddTransient<IIntrinioForexCurrencyService, IntrinioForexCurrencyService>();
            serviceCollection.AddTransient<ICurrencyDataRepository, CurrencyDataRepository>();
            serviceCollection.AddTransient<ICurrencyDataService, CurrencyDataService>();

            serviceCollection.AddTransient<IPipelineManagerService, PipelineManagerService>();
            serviceCollection.AddPipelineProcessors(pipelineSettings);
        }
    }
}
