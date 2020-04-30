using Stocks.Ameritrade.Models;
using Stocks.Ameritrade.Models.PriceHistory;
using Stocks.Ameritrade.Services.Interfaces;
using Stocks.Ameritrade.Tests.Fixtures;
using Stocks.Common.Models;
using Stocks.Common.Tests;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Stocks.Ameritrade.Tests.Services
{
    public class AmeritradeApiServiceTests : IClassFixture<AmeritradeFixture>
    {
        private readonly IAmeritradeApiService _ameritradeApiService;
        private readonly ITestOutputHelper _testOutputHelper;

        public AmeritradeApiServiceTests(AmeritradeFixture ameritradeFixture, ITestOutputHelper testOutputHelper)
        {
            if(ameritradeFixture == null)
            {
                throw new ArgumentNullException(nameof(ameritradeFixture));
            }

            _ameritradeApiService = ameritradeFixture.AmeritradeApiService;
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task GetPriceHistory_InvalidParameters_Error()
        {
            await Assert.ThrowsAsync<ArgumentException>(() => _ameritradeApiService.GetPriceHistoryAsync(null, null)).ConfigureAwait(false);
            await Assert.ThrowsAsync<ArgumentException>(() => _ameritradeApiService.GetPriceHistoryAsync(string.Empty, null)).ConfigureAwait(false);
            await Assert.ThrowsAsync<ArgumentNullException>(() => _ameritradeApiService.GetPriceHistoryAsync("symbol", null)).ConfigureAwait(false);
            await _ameritradeApiService.GetPriceHistoryAsync("symbol", new AmeritradePriceHistoryRequest()).ConfigureAwait(false);
        }

        [Fact]
        public async Task GetPriceHistory_BadRequest()
        {
            string symbol = "NFLX";
            var priceHistoryRequest = new AmeritradePriceHistoryRequest()
            {
                PeriodType = AmeritradePeriodType.Year,
                FrequencyType = AmeritradeFrequencyType.Daily,
                Period = 12
            };
            ServiceResponse<AmeritradePriceHistoryResponse> response = await _ameritradeApiService.GetPriceHistoryAsync(symbol, priceHistoryRequest).ConfigureAwait(false);
            Assert.False(response.IsSuccess);
            Assert.NotEmpty(response.ErrorMessage);
            Assert.Null(response.Content);
            _testOutputHelper.WriteLine(response.ToPrettyJson());
        }

        [Fact]
        public async Task GetPriceHistory_Ok()
        {
            string symbol = "NFLX";
            var priceHistoryRequest = new AmeritradePriceHistoryRequest() 
            {
                PeriodType = AmeritradePeriodType.Year,
                FrequencyType = AmeritradeFrequencyType.Daily,
                Period = 1
            };
            ServiceResponse<AmeritradePriceHistoryResponse> response = await _ameritradeApiService.GetPriceHistoryAsync(symbol, priceHistoryRequest).ConfigureAwait(false);
            Assert.True(response.IsSuccess, response.ErrorMessage);
            Assert.Null(response.ErrorMessage);
            Assert.NotNull(response.Content);
            Assert.NotNull(response.Content.Candles);
            _testOutputHelper.WriteLine(response.ToPrettyJson());
        }
    }
}
