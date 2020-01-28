using Microsoft.Extensions.Configuration;
using Stocks.Data.Models;
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
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.json.user", true)
                .Build();
            var connectionStringsSection = configuration.GetSection("ConnectionStrings");
            var connectionStrings = new ConnectionStrings();
            connectionStringsSection.Bind(connectionStrings);

            var stocksContext = new StocksContext(connectionStrings.Stocks);
            PriceHistoryRepository = new PriceHistoryRepository(stocksContext);
        }
    }
}
