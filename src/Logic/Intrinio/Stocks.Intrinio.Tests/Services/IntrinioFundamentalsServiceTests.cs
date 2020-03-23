using Stocks.Intrinio.Services;
using Stocks.Intrinio.Services.Interfaces;
using Stocks.Intrinio.Tests.Fixtures;
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

            _testOutputHelper.WriteLine("Test fundementals details, standardized intrinio");
            _testOutputHelper.WriteLine(_fundamentalsService.GetStandardizedFundementals("AAPL-income_statement-2018-Q1"));

            _testOutputHelper.WriteLine("Test fundementals details, by company");
            _testOutputHelper.WriteLine(_fundamentalsService.GetStandardizedFundementals("AAPL"));
        }
    }
}
