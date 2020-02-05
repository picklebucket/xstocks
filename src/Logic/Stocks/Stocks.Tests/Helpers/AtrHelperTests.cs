using Stocks.Data.Entities;
using Stocks.Data.Repositories.Interfaces;
using Stocks.Helpers;
using Stocks.Models;
using Stocks.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Stocks.Tests.Helpers
{
    [Collection(Constants.CollectionName)]
    public class AtrHelperTests
    {
        private readonly IPriceHistoryRepository _priceHistoryRepository;

        public AtrHelperTests(HelpersFixture helpersFixture)
        {
            _priceHistoryRepository = helpersFixture.PriceHistoryRepository;
        }

        [Fact]
        public async Task CalculateAtr()
        {
            string symbol = "NFLX";
            PriceHistory priceHistory = await _priceHistoryRepository.GetAsync(symbol);
            var candles = new List<CandleModel>();
            foreach (var candle in priceHistory.Candles)
            {
                candles.Add(new CandleModel()
                {
                    Date = DateTimeOffset.FromUnixTimeMilliseconds(candle.DateTimeUnixEpochMilliseconds),
                    Open = candle.Open,
                    Close = candle.Close,
                    High = candle.High,
                    Low = candle.Low,
                    Volume = candle.Volume
                });
            }
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
