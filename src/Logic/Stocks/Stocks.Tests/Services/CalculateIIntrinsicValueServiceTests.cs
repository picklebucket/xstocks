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

    public class CalculateIIntrinsicValueServiceTests : BaseTests
    {
        private readonly ICalculateIIntrinsicValueService _intrinsicValueService;
        private readonly ITestOutputHelper _testOutputHelper;

        public CalculateIIntrinsicValueServiceTests(StocksFixture stocksFixture, ITestOutputHelper testOutputHelper) : base(stocksFixture)
        {
            _intrinsicValueService = stocksFixture.ServiceProvider.GetService<ICalculateIIntrinsicValueService>();
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task Calculate()
        {
            var value = await _intrinsicValueService.CalculateIntrinsicValueAsync("AAPL", 2018, FiscalPeriod.Q1).ConfigureAwait(false);
            Assert.NotNull(value);
            _testOutputHelper.WriteLine(value.ToString());
        }
    }
}
