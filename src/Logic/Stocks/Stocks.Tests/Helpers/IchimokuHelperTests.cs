using Stocks.Data.Entities;
using Stocks.Data.Repositories.Interfaces;
using Stocks.Helpers;
using Stocks.Models;
using Stocks.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Stocks.Tests.Helpers
{
    [Collection(Constants.CollectionName)]
    public class IchimokuHelperTests 
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IPriceHistoryRepository _priceHistoryRepository;

        public IchimokuHelperTests(HelpersFixture helpersFixture, ITestOutputHelper testOutputHelper)
        {
            _priceHistoryRepository = helpersFixture.PriceHistoryRepository;
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task BuildIchimokuCloud()
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
            var cloudItems = IchimokuItemWrapper.Create(IchimokuHelper.BuildIchimokuCloud(candles));
            _testOutputHelper.WriteLine(JsonSerializer.Serialize(cloudItems, new JsonSerializerOptions() { WriteIndented = true}));

            Assert.Equal(candles.Count + IchimokuHelper.MediumShiftSize, cloudItems.Count);

            //tenkan-sen has no values for first 8 and last 25 periods
            for (int i = 0; i < IchimokuHelper.SmallShiftSize; i++)
            {
                Assert.Null(cloudItems[i].TenkanSen);
            }
            Assert.NotNull(cloudItems[IchimokuHelper.SmallShiftSize].TenkanSen);

            int startIndex = cloudItems.Count - 1;
            int endIndex = cloudItems.Count - IchimokuHelper.MediumShiftSize - 1;
            for (int i = startIndex; i > endIndex; i--)
            {
                Assert.Null(cloudItems[i].TenkanSen);
            }
            Assert.NotNull(cloudItems[endIndex].TenkanSen);

            //kijun-sen has no values for first 26 and last 25 periods
            for (int i = 0; i < IchimokuHelper.MediumShiftSize; i++)
            {
                Assert.Null(cloudItems[i].KijunSen);
            }
            Assert.NotNull(cloudItems[IchimokuHelper.MediumShiftSize].KijunSen);

            startIndex = cloudItems.Count - 1;
            endIndex = cloudItems.Count - IchimokuHelper.MediumShiftSize - 1;
            for (int i = startIndex; i > endIndex; i--)
            {
                Assert.Null(cloudItems[i].KijunSen);
            }
            Assert.NotNull(cloudItems[endIndex].KijunSen);

            //chikou span has no values for last 50 periods
            startIndex = cloudItems.Count - 1;
            endIndex = cloudItems.Count - 2 * IchimokuHelper.MediumShiftSize - 1;
            for (int i = startIndex; i > endIndex; i--)
            {
                Assert.Null(cloudItems[i].ChikouSpan);
            }
            Assert.NotNull(cloudItems[endIndex].ChikouSpan);

            //senkou span a has no values for first 50 periods
            endIndex = 2 * IchimokuHelper.MediumShiftSize;
            for (int i = 0; i < endIndex; i++)
            {
                Assert.Null(cloudItems[i].SenkouSpanA);
            }
            Assert.NotNull(cloudItems[endIndex].SenkouSpanA);

            //senkou span b has no values for first 76 periods
            endIndex = IchimokuHelper.LargeShiftSize + IchimokuHelper.MediumShiftSize;
            for (int i = 0; i < endIndex; i++)
            {
                Assert.Null(cloudItems[i].SenkouSpanB);
            }
            Assert.NotNull(cloudItems[endIndex].SenkouSpanB);
        }
    }

    public class IchimokuItemWrapper : IchimokuItem
    {
        public int Number { get; set; }

        public IchimokuItemWrapper(int number, DateTimeOffset date, double? tenkanSen, double? kijunSen, double? chikouSpan, double? senkouSpanA, double? senkouSpanB) :
            base(date, tenkanSen, kijunSen, chikouSpan, senkouSpanA, senkouSpanB)
        {
            Number = number;
        }

        public static List<IchimokuItemWrapper> Create(List<IchimokuItem> items)
        {
            return items.Select((x, i) => new IchimokuItemWrapper(i + 1, x.Date, x.TenkanSen, x.KijunSen, x.ChikouSpan, x.SenkouSpanA, x.SenkouSpanB)).ToList();
        }
    }
}
