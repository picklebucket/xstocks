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
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.json.user", true)
                .Build();
            string connectionString = configuration.GetSection("ConnectionStrings").GetSection("Stocks").Value;

            var stocksContext = new StocksContext(connectionString);
            PriceHistoryRepository = new PriceHistoryRepository(stocksContext);
        }
    }
}
