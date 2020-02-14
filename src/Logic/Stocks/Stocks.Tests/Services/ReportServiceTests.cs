using Stocks.Services.Interfaces;
using Stocks.Tests.Fixtures;
using Xunit;
using Stocks.Services;
using Stocks.Tests.Mock;
using System.Threading.Tasks;
using System.Text;
using Xunit.Abstractions;

namespace Stocks.Tests.Services
{
    [Collection(Constants.CollectionName)]
    public class ReportServiceTests : BaseTests
    {
        private readonly IReportService _reportService;
        private readonly ITestOutputHelper _testOutputHelper;

        public ReportServiceTests(StocksFixture stocksFixture, ITestOutputHelper testOutputHelper) : base(stocksFixture)
        {
            _reportService = new ReportService(new FilePriceHistoryRepository());
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task BuildReport()
        {
            string symbol = "NFLX";
            var candles = await _reportService.GetCollectedCandlesAsync(symbol);
            var reportItems = _reportService.BuildReport(symbol, candles);
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
        }

        [Fact]
        public async Task ExportToCsv()
        {
            string symbol = "NFLX";
            var candles = await _reportService.GetCollectedCandlesAsync(symbol);
            var reportItems = _reportService.BuildReport(symbol, candles);
            var csvContent = _reportService.ExportToIndicatorCsv(reportItems);
            string output = Encoding.Default.GetString(csvContent);
            _testOutputHelper.WriteLine(output);
        }
    }
}
