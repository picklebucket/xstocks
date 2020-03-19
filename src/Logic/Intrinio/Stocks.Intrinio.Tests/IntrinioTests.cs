using Stocks.Intrinio.Models;
using Stocks.Intrinio.Services;
using Stocks.Intrinio.Tests.Fixtures;
using Stocks.Intrinio.Wrappers;
using Xunit;
using Xunit.Abstractions;

namespace Stocks.Intrinio.Tests
{
    [Collection(TestConstants.IntrinioCollectionName)]
    public class IntrinioTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IntrinioSecurityApi _intrinioSecurityApi;
        private readonly IntrinioFundamentalsApi _intrinioFundamentalsApi;

        public IntrinioTests(IntrinioFixture intrinioFixture, ITestOutputHelper testOutputHelper)
        {
            _intrinioSecurityApi = intrinioFixture.IntrinioSecurityApi;
            _intrinioFundamentalsApi = intrinioFixture.IntrinioFundamentalsApi;
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Intrinio()
        {
            _testOutputHelper.WriteLine("Test securities intrinio");

            SecuritiesService i = new SecuritiesService(_intrinioSecurityApi);
            _testOutputHelper.WriteLine(i.GetTestSignal("AAPL"));

            _testOutputHelper.WriteLine("Test fundementals intrinio");
            FundementalsService f = new FundementalsService(_intrinioFundamentalsApi);
            _testOutputHelper.WriteLine(f.GetTestSignal("AAPL"));

            _testOutputHelper.WriteLine("Test fundementals details intrinio");
            _testOutputHelper.WriteLine(f.GetReportedFinancialsByTicker("AAPL-income_statement-2018-Q1"));

            _testOutputHelper.WriteLine("Test fundementals details, standardized intrinio");
            _testOutputHelper.WriteLine(f.GetStandardizedFundementals("AAPL-income_statement-2018-Q1"));

            _testOutputHelper.WriteLine("Test fundementals details, by company");
            _testOutputHelper.WriteLine(f.GetStandardizedFundementals("AAPL"));
        }
    }
}
