using Stocks.Helpers;
using Stocks.Services.Interfaces;
using Stocks.Tests.Fixtures;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;

namespace Stocks.Tests.Helpers
{
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
            string symbol = "NFLX";
            var candles = await _priceHistoryService.GetCollectedCandlesAsync(symbol).ConfigureAwait(false);
            var atrItems = AtrHelper.CalculateATR(candles);
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
