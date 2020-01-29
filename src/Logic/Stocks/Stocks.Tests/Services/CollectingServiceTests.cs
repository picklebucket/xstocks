using Stocks.Ameritrade.Models.PriceHistory;
using Stocks.Data.Entities;
using Stocks.Data.Repositories.Interfaces;
using Stocks.Services.Interfaces;
using Stocks.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Stocks.Tests.Services
{
    public class CollectingServiceTests : IClassFixture<StocksFixture>
    {
        private readonly ICollectingService _collectingService;
        private readonly IPriceHistoryRepository _priceHistoryRepository;

        public CollectingServiceTests(StocksFixture stocksFixture)
        {
            _collectingService = stocksFixture.CollectingService;
            _priceHistoryRepository = stocksFixture.PriceHistoryRepository;
        }

        [Fact]
        public async Task CollectData()
        {
            string symbol = "NFLX";
            var priceHistoryRequest = new PriceHistoryRequest()
            {
                PeriodType = PeriodType.Year,
                FrequencyType = FrequencyType.Daily,
                Period = 1
            };
            await _collectingService.CollectDataAsync(symbol, priceHistoryRequest);

            PriceHistory priceHistory = _priceHistoryRepository.GetAsync(symbol).Result;
            Assert.NotNull(priceHistory);
            Assert.NotEmpty(priceHistory.Candles);
        }
    }
}
