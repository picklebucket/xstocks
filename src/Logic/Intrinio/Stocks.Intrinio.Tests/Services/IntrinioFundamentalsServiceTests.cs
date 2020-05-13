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
            //_testOutputHelper.WriteLine("Test fundementals details, by company");
            //_testOutputHelper.WriteLine(_fundamentalsService.GetStandardizedFundementals("AAPL"));
        }

        [Fact]
        public void GetReportedFinancials()
        {
            _testOutputHelper.WriteLine(_fundamentalsService.GetReportedFinancialsByTicker("AAPL-income_statement-2018-Q1"));
        }

        [Fact]
        public async Task GetStandartizedFundamentals_Cashflow()
        {
            var response = await _fundamentalsService.GetStandardizedFundementalsAsync("AAPL", StatementCode.CashflowStatement, 2018, FiscalPeriod.Q1).ConfigureAwait(false);
            Assert.True(response.IsSuccess);
            Assert.NotNull(response.Content);
            Assert.NotEmpty(response.Content.StandardizedFinancials);
            _testOutputHelper.WriteLine(response.Content.ToJson());
        }

        [Fact]
        public async Task GetStandartizedFundamentals_BalanceSheet()
        {
            var response = await _fundamentalsService.GetStandardizedFundementalsAsync("AAPL", StatementCode.BalanceSheetStatement, 2018, FiscalPeriod.Q1).ConfigureAwait(false);
            Assert.True(response.IsSuccess);
            Assert.NotNull(response.Content);
            Assert.NotEmpty(response.Content.StandardizedFinancials);
            _testOutputHelper.WriteLine(response.Content.ToJson());
        }

        [Fact]
        public async Task GetStandartizedFundamentals_Calculations()
        {
            var response = await _fundamentalsService.GetStandardizedFundementalsAsync("AAPL", StatementCode.Calculations, 2018, FiscalPeriod.Q1).ConfigureAwait(false);
            Assert.True(response.IsSuccess);
            Assert.NotNull(response.Content);
            Assert.NotEmpty(response.Content.StandardizedFinancials);
            _testOutputHelper.WriteLine(response.Content.ToJson());
        }

        [Fact]
        public async Task GetStandartizedFundamentals_Income()
        {
            var response = await _fundamentalsService.GetStandardizedFundementalsAsync("AAPL", StatementCode.IncomeStatement, 2018, FiscalPeriod.Q1).ConfigureAwait(false);
            Assert.True(response.IsSuccess);
            Assert.NotNull(response.Content);
            Assert.NotEmpty(response.Content.StandardizedFinancials);
            _testOutputHelper.WriteLine(response.Content.ToJson());
        }
    }
}
