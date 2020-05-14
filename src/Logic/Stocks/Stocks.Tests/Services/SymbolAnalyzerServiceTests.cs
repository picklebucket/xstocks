using Microsoft.Extensions.DependencyInjection;
using Stocks.Intrinio.Models;
using Stocks.Services.Interfaces;
using Stocks.Tests.Fixtures;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Stocks.Tests.Services
{
    [Collection(Constants.CollectionName)]
    public class SymbolAnalyzerServiceTests : BaseTests
    {
        private readonly ISymbolAnalyzerService _symbolAnalyzerService;
        private readonly ITestOutputHelper _testOutputHelper;

        public SymbolAnalyzerServiceTests(StocksFixture stocksFixture, ITestOutputHelper testOutputHelper) 
            : base(stocksFixture)
        {
            _symbolAnalyzerService = stocksFixture.ServiceProvider.GetService<ISymbolAnalyzerService>();
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task Calculate()
        {
            var value = await _symbolAnalyzerService.AnalyzeSymbolBasedOnIntrinsicValueAsync("AAPL", 2018, FiscalPeriod.Q1).ConfigureAwait(false);
            Assert.NotNull(value);
            _testOutputHelper.WriteLine(value.ToString());
        }
    }
}
