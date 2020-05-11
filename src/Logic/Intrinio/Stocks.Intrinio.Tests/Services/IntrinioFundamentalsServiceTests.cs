using Stocks.Common.Tests;
using Stocks.Intrinio.Models;
using Stocks.Intrinio.Services;
using Stocks.Intrinio.Services.Interfaces;
using Stocks.Intrinio.Tests.Fixtures;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Stocks.Intrinio.Tests.Services
{
    [Collection(TestConstants.IntrinioCollectionName)]
    public class IntrinioFundamentalsServiceTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IIntrinioFundementalsService _fundamentalsService;

        public IntrinioFundamentalsServiceTests(IntrinioFixture intrinioFixture, ITestOutputHelper testOutputHelper)
        {
            _fundamentalsService = new IntrinioFundementalsService(intrinioFixture.IntrinioFundamentalsApi);
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void TestFundamentals()
        {
            _testOutputHelper.WriteLine("Test fundementals intrinio");
            _testOutputHelper.WriteLine(_fundamentalsService.GetTestSignal("AAPL"));

            _testOutputHelper.WriteLine("Test fundementals details intrinio");
            _testOutputHelper.WriteLine(_fundamentalsService.GetReportedFinancialsByTicker("AAPL-income_statement-2018-Q1"));

            //_testOutputHelper.WriteLine("Test fundementals details, by company");
            //_testOutputHelper.WriteLine(_fundamentalsService.GetStandardizedFundementals("AAPL"));
        }

        [Fact]
        public async Task GetStandartizedFundamentals()
        {
            _testOutputHelper.WriteLine("Test fundementals details, standardized intrinio");
            var response = await _fundamentalsService.GetStandardizedFundementalsAsync("AAPL", StatementCode.CashflowStatement, 2018, FiscalPeriod.Q1).ConfigureAwait(false);
            Assert.True(response.IsSuccess);
            Assert.NotNull(response.Content);
            Assert.NotEmpty(response.Content.StandardizedFinancials);
            _testOutputHelper.WriteLine(response.Content.ToJson());
        }
    }
}
