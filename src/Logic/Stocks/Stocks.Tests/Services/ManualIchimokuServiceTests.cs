using Stocks.Data.Entities;
using Stocks.Data.Repositories.Interfaces;
using Stocks.Helpers;
using Stocks.Models;
using Stocks.Services;
using Stocks.Services.Ichimoku;
using Stocks.Services.Interfaces;
using Stocks.Tests.Mock;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Stocks.Tests.Fixtures;
using Stocks.Intrinio.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Stocks.Models.Api.Ichimoku;
using Stocks.Models.Api;

namespace Stocks.Tests.Helpers
{
    [Collection(Constants.CollectionName)]
    public class ManualIchimokuServiceTests : BaseTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IIchimokuService _ichimokuService;

        public ManualIchimokuServiceTests(ITestOutputHelper testOutputHelper, StocksFixture stocksFixture)
            :base(stocksFixture)
        {
            _ichimokuService = new ManualIchimokuService(ServiceProvider.GetService<IIntrinioSecuritiesService>());
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task BuildIchimokuCloud()
        {
            var request = new IchimokuRequest("AAPL", new DateTime(2018, 1, 1), new DateTime(2019, 1, 1), FrequencyType.Daily);
            var serviceResponse = await _ichimokuService.BuildIchimokuCloudAsync(request).ConfigureAwait(false);
            Assert.True(serviceResponse.IsSuccess, serviceResponse.ErrorMessage);
            var cloudItems = IchimokuItemWrapper.Create(serviceResponse.Content.Items);
            _testOutputHelper.WriteLine(JsonSerializer.Serialize(cloudItems, new JsonSerializerOptions() { WriteIndented = true}));

            //tenkan-sen has no values for first 8 and last 25 periods
            for (int i = 0; i < IchimokuConstants.SmallShiftSize; i++)
            {
                Assert.Null(cloudItems[i].TenkanSen);
            }
            Assert.NotNull(cloudItems[IchimokuConstants.SmallShiftSize].TenkanSen);

            int startIndex = cloudItems.Count - 1;
            int endIndex = cloudItems.Count - IchimokuConstants.MediumShiftSize - 1;
            for (int i = startIndex; i > endIndex; i--)
            {
                Assert.Null(cloudItems[i].TenkanSen);
            }
            Assert.NotNull(cloudItems[endIndex].TenkanSen);

            //kijun-sen has no values for first 26 and last 25 periods
            for (int i = 0; i < IchimokuConstants.MediumShiftSize; i++)
            {
                Assert.Null(cloudItems[i].KijunSen);
            }
            Assert.NotNull(cloudItems[IchimokuConstants.MediumShiftSize].KijunSen);

            startIndex = cloudItems.Count - 1;
            endIndex = cloudItems.Count - IchimokuConstants.MediumShiftSize - 1;
            for (int i = startIndex; i > endIndex; i--)
            {
                Assert.Null(cloudItems[i].KijunSen);
            }
            Assert.NotNull(cloudItems[endIndex].KijunSen);

            //chikou span has no values for last 50 periods
            startIndex = cloudItems.Count - 1;
            endIndex = cloudItems.Count - 2 * IchimokuConstants.MediumShiftSize - 1;
            for (int i = startIndex; i > endIndex; i--)
            {
                Assert.Null(cloudItems[i].ChikouSpan);
            }
            Assert.NotNull(cloudItems[endIndex].ChikouSpan);

            //senkou span a has no values for first 50 periods
            endIndex = 2 * IchimokuConstants.MediumShiftSize;
            for (int i = 0; i < endIndex; i++)
            {
                Assert.Null(cloudItems[i].SenkouSpanA);
            }
            Assert.NotNull(cloudItems[endIndex].SenkouSpanA);

            //senkou span b has no values for first 76 periods
            endIndex = IchimokuConstants.LargeShiftSize + IchimokuConstants.MediumShiftSize;
            for (int i = 0; i < endIndex; i++)
            {
                Assert.Null(cloudItems[i].SenkouSpanB);
            }
            Assert.NotNull(cloudItems[endIndex].SenkouSpanB);
        }

        //[Fact]
        //public async Task CalculateBuySignals()
        //{
        //    string symbol = "NFLX";
        //    var candles = await _reportService.GetCollectedCandlesAsync(symbol).ConfigureAwait(false);
        //    var ichimokuItems = SignalHelper.BuildIchimokuCloud(candles);
        //    var signals = SignalHelper.GetSignals(ichimokuItems, candles);
        //    Assert.NotEmpty(signals);
        //    var buySignals = signals.Where(x => x == SignalType.BuySignal);
        //    Assert.NotEmpty(buySignals);
        //}
    }
}
