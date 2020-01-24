using Stocks.Ameritrade.Models.PriceHistory;
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

        public CollectingServiceTests(StocksFixture stocksFixture)
        {
            _collectingService = stocksFixture.CollectingService;
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
        }
    }
}
