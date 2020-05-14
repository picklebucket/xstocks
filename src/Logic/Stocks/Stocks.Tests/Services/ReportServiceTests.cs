using Stocks.Services.Interfaces;
using Stocks.Tests.Fixtures;
using Xunit;
using Stocks.Services;
using Stocks.Tests.Mock;
using System.Threading.Tasks;
using System.Text;
using Xunit.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using Stocks.Services.Collecting;
using System;
using Stocks.Models.Api.Ichimoku;
using Stocks.Models.Api;
using Stocks.Models.Api.PriceHistory;
using Stocks.Services.Ichimoku;
using System.Linq;
using Stocks.Models;
using Stocks.Common.Tests;

namespace Stocks.Tests.Services
{
    [Collection(Constants.CollectionName)]
    public class ReportServiceTests : BaseTests
    {
        private readonly IReportService _reportService;
        private readonly ITestOutputHelper _testOutputHelper;

        public ReportServiceTests(StocksFixture stocksFixture, ITestOutputHelper testOutputHelper) : base(stocksFixture)
        {
            _reportService = ServiceProvider.GetService<IReportService>();
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task Compare()
        {
            string symbol = "AAPL";
            DateTime startDate = new DateTime(2019, 1, 1);
            DateTime endDate = new DateTime(2020, 1, 1);
            var frequencyType = FrequencyType.Daily;
            var ichimokuRequest = new IchimokuRequest(symbol, startDate, endDate, frequencyType);
            var ichimokuService = ServiceProvider.GetService<IIchimokuService>();
            var ichimokuResponse = await ichimokuService.BuildIchimokuCloudAsync(ichimokuRequest).ConfigureAwait(false);
            var ichimokuItems = ichimokuResponse.Content.Items;
            _testOutputHelper.WriteLine("Ichimoku");
            _testOutputHelper.WriteLine(ichimokuItems.Select(x => x.Date).Take(10).ToPrettyJson());

            var priceHistoryRequest = new PriceHistoryRequest(symbol, startDate, endDate, frequencyType);
            var priceHistoryService = ServiceProvider.GetService<IPriceHistoryService>();
            var priceHistoryResponse = await priceHistoryService.GetPriceHistoryAsync(priceHistoryRequest).ConfigureAwait(false);
            var candles = priceHistoryResponse.Content.CandleItems;
            _testOutputHelper.WriteLine("Candles");
            _testOutputHelper.WriteLine(candles.Select(x => x.Date).Take(10).ToPrettyJson());
        }

        [Fact]
        public async Task BuildReport()
        {
            var reportItems = await _reportService.BuildReportAsync("AAPL", new DateTime(2019, 1, 1), new DateTime(2020, 1, 1), FrequencyType.Daily).ConfigureAwait(false);
            Assert.NotNull(reportItems);
            Assert.NotEmpty(reportItems);
            foreach (var reportItem in reportItems)
            {
                Assert.NotNull(reportItem.Symbol);
                Assert.NotEmpty(reportItem.Symbol);
                Assert.NotNull(reportItem.AtrItem);
                Assert.NotNull(reportItem.CandleItem);
                Assert.NotNull(reportItem.IchimokuItem);
                Assert.NotNull(reportItem.IndicatorItem);
            }
            var buySignalsCount = reportItems.Count(x => x.SignalType == SignalType.BullishStrong);
            Assert.True(buySignalsCount > 0);
        }        

        [Fact]
        public async Task ExportToIndicatorCsv()
        {
            var reportItems = await _reportService.BuildReportAsync("AAPL", new DateTime(2018, 1, 1), new DateTime(2019, 1, 1), FrequencyType.Daily).ConfigureAwait(false);
            var csvContent = _reportService.ExportToIndicatorCsv(reportItems);
            string output = Encoding.Default.GetString(csvContent);
            _testOutputHelper.WriteLine(output);
        }

        [Fact]
        public async Task ExportToTradeCsv()
        {
            var reportItems = await _reportService.BuildReportAsync("AAPL", new DateTime(2018, 1, 1), new DateTime(2019, 1, 1), FrequencyType.Daily).ConfigureAwait(false);
            var csvContent = _reportService.ExportToTradeCsv(reportItems);
            string output = Encoding.Default.GetString(csvContent);
            _testOutputHelper.WriteLine(output);
        }

        [Fact]
        public async Task ExportToGoodCompaniesCsv()
        {
            var reportItems = await _reportService.BuildReportBasedOnIntrinsicValue().ConfigureAwait(false);
            var csvContent = _reportService.ExportGoodPriceCompaniesToCsv(reportItems);
            string output = Encoding.Default.GetString(csvContent);
            _testOutputHelper.WriteLine(output);
        }
    }
}
