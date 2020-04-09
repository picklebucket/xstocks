using Stocks.Common.Tests;
using Stocks.Intrinio.Models.Ichimoku;
using Stocks.Intrinio.Models.PriceHistory;
using Stocks.Intrinio.Services;
using Stocks.Intrinio.Services.Interfaces;
using Stocks.Intrinio.Tests.Fixtures;
using Stocks.Intrinio.Wrappers;
using System;
using System.Collections.Generic;
using System.Linq;
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
            var request = new IntrinioIchimokuRequest("AAPL", new DateTime(2018, 1, 1), new DateTime(2019, 1, 1));
            var firstPageResponse = await _securitiesService.GetIchimokuAsync(request).ConfigureAwait(false);
            Assert.True(firstPageResponse.IsSuccess, firstPageResponse.ErrorMessage);
            Assert.NotEmpty(firstPageResponse.Content.Technicals);
            Assert.NotNull(firstPageResponse.Content.NextPage);
            _testOutputHelper.WriteLine(firstPageResponse.Content.ToPrettyJson());

            request.NextPage = firstPageResponse.Content.NextPage;
            var secondPageResponse = await _securitiesService.GetIchimokuAsync(request).ConfigureAwait(false);
            Assert.True(secondPageResponse.IsSuccess, secondPageResponse.ErrorMessage);
            Assert.NotEmpty(secondPageResponse.Content.Technicals);
            Assert.NotNull(secondPageResponse.Content.NextPage);
            Assert.NotEqual(firstPageResponse.Content.Technicals.First().Date, secondPageResponse.Content.Technicals.First().Date);

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
