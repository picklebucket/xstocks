using Stocks.Helpers;
using Stocks.Services.Interfaces;
using Stocks.Tests.Fixtures;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Stocks.Services.Collecting;
using Stocks.Models.Api.PriceHistory;
using System;
using Stocks.Models.Api;

namespace Stocks.Tests.Helpers
{
    [Collection(Constants.CollectionName)]
    public class AtrHelperTests : BaseTests
    {
        private readonly IPriceHistoryService _priceHistoryService;

        public AtrHelperTests(StocksFixture stocksFixture) : base(stocksFixture) 
        {
            _priceHistoryService = ServiceProvider.GetService<IPriceHistoryService>();
        }

        [Fact]
        public async Task CalculateAtr()
        {
            var priceHistoryRequest = new PriceHistoryRequest("AAPL", new DateTime(2018, 1, 1), new DateTime(2019, 1, 1), FrequencyType.Daily);
            var serviceResponse = await _priceHistoryService.GetPriceHistoryAsync(priceHistoryRequest).ConfigureAwait(false);

            var atrItems = AtrHelper.CalculateATR(serviceResponse.Content.CandleItems);
            int i;
            for (i = 0; i < 13; i++)
            {
                Assert.NotNull(atrItems[i].TR);
                Assert.Null(atrItems[i].ATR);
            }
            for (; i < atrItems.Count; i++)
            {
                Assert.NotNull(atrItems[i].TR);
                Assert.NotNull(atrItems[i].ATR);
            }
        }
    }
}
