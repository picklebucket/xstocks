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
using Stocks.ML.SentimentAnalysis;
using Stocks.NewsApi.Models;
using Stocks.NewsApi.Services;
using Stocks.Services;
using Stocks.Services.Collecting;
using Stocks.Services.Ichimoku;
using Stocks.Services.Interfaces;

namespace Stocks.UI.DI
{
    public static class DIConfiguration
    {
        public static void ConfigureServices(IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var newsApiSettings = new NewsApiConfiguration();
            var newsConfigSection = configuration.GetSection("NewsApi");
            newsConfigSection.Bind(newsApiSettings);
            serviceCollection.AddSingleton(newsApiSettings);

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
            serviceCollection.AddTransient<INewsDataService, NewsDataService>();
            serviceCollection.AddTransient<INewsDetailsRepository, NewsDetailsRepository>();
            serviceCollection.AddTransient<INewsApiService, NewsApiService>();

            serviceCollection.AddSingleton<ISentimentAnalysisService, SentimentAnalysisService>();
        }
    }
}
