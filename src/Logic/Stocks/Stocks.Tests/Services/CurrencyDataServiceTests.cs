using Microsoft.Extensions.DependencyInjection;
using Stocks.Services;
using Stocks.Services.Interfaces;
using Stocks.Tests.Fixtures;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Stocks.Tests.Services
{
    [Collection(Constants.CollectionName)]
    public class CurrencyDataServiceTests : BaseTests
    {
        private static readonly List<string> _currencyPairs = new List<string>() { "EURUSD", "USDCHF", "USDJPY", "GBPUSD", "EURCHF" };

        private readonly ICurrencyDataService _currencyDataService;
        private readonly ITestOutputHelper _testOutputHelper;

        public CurrencyDataServiceTests(StocksFixture stocksFixture, ITestOutputHelper testOutputHelper)
            : base(stocksFixture)
        {
            _currencyDataService = stocksFixture.ServiceProvider.GetService<ICurrencyDataService>();
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task SyncData()
        {
            foreach (var pair in _currencyPairs)
            {
                var value = await _currencyDataService.SyncDataFor7days15minsInterval(pair).ConfigureAwait(false);
                Assert.NotNull(value);
                _testOutputHelper.WriteLine(value.ToString());
                Thread.Sleep(60000);
            }
        }

        [Fact]
        public async Task ReportROC()
        {
            await _currencyDataService.ReportToExcel(_currencyPairs).ConfigureAwait(false);
        }
    }
}
