using Microsoft.Extensions.Configuration;
using Stocks.Data.Repositories;
using Stocks.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Data.Tests.Fixtures
{
    public class DataFixture
    {
        public IPriceHistoryRepository PriceHistoryRepository { get; }

        public DataFixture()
        {
            var dataSettings = DataSettings.GetSettings();
            var stocksContext = new StocksContext(dataSettings.Stocks);
            PriceHistoryRepository = new PriceHistoryRepository(stocksContext);
        }
    }
}
