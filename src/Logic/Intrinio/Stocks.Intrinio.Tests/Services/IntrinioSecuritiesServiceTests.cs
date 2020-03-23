using Stocks.Common.Tests;
using Stocks.Intrinio.Models;
using Stocks.Intrinio.Models.PriceHistory;
using Stocks.Intrinio.Services;
using Stocks.Intrinio.Services.Interfaces;
using Stocks.Intrinio.Tests.Fixtures;
using Stocks.Intrinio.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Stocks.Intrinio.Tests.Services
{
    [Collection(TestConstants.IntrinioCollectionName)]
    public class IntrinioSecuritiesServiceTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IIntrinioSecuritiesService _securitiesService;

        public IntrinioSecuritiesServiceTests(IntrinioFixture intrinioFixture, ITestOutputHelper testOutputHelper)
        {
            _securitiesService = new IntrinioSecuritiesService(intrinioFixture.IntrinioSecurityApi);
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task GetIchimoku()
        {
            var request = new IchimokuRequest("AAPL", new DateTime(2018, 1, 1), new DateTime(2019, 1, 1));
            var response = await _securitiesService.GetIchimokuAsync(request).ConfigureAwait(false);
            Assert.True(response.IsSuccess, response.ErrorMessage);
            Assert.NotEmpty(response.Content.Technicals);
            _testOutputHelper.WriteLine(response.Content.ToPrettyJson());
        }

        [Fact]
        public async Task GetPriceHistory()
        {
            var request = new IntrinioPriceHistoryRequest("AAPL", new DateTime(2018, 1, 1), new DateTime(2019, 1, 1), IntrinioFrequencyType.Daily);
            var response = await _securitiesService.GetStockPricesAsync(request).ConfigureAwait(false);
            Assert.True(response.IsSuccess, response.ErrorMessage);
            Assert.NotEmpty(response.Content.StockPrices);
            _testOutputHelper.WriteLine(response.Content.ToPrettyJson());
        }

    }
}
