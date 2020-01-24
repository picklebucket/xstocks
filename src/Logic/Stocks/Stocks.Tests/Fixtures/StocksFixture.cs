using Stocks.Ameritrade.Models;
using Stocks.Ameritrade.Services;
using Stocks.Ameritrade.Services.Interfaces;
using Stocks.Ameritrade.Tests;
using Stocks.Data;
using Stocks.Data.Repositories;
using Stocks.Data.Repositories.Interfaces;
using Stocks.Data.Tests;
using Stocks.Services;
using Stocks.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Tests.Fixtures
{
    public class StocksFixture
    {
        public ICollectingService CollectingService { get; }
        public StocksFixture()
        {
            var apiSettings = AmeritradeSettings.GetSettings();
            IAmeritradeApiService ameritradeApiService = new AmeritradeApiService(apiSettings);

            var dataSettings = DataSettings.GetSettings();
            var stocksContext = new StocksContext(dataSettings.Stocks);
            IPriceHistoryRepository priceHistoryRepository = new PriceHistoryRepository(stocksContext);

            CollectingService = new CollectingService(ameritradeApiService, priceHistoryRepository);
        }
    }
}
