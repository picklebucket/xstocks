using Stocks.Data.Entities;
using Stocks.Data.Repositories.Interfaces;
using Stocks.Tests.Fixtures;
using System;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Stocks.Services.Collecting;
using Stocks.Intrinio.Services.Interfaces;
using Stocks.Models.Api.PriceHistory;
using Stocks.Models.Api;

namespace Stocks.Tests.Services
{
    [Collection(Constants.CollectionName)]
    public class IntrinioPriceHistoryServiceTests : BaseTests
    {
        private readonly IPriceHistoryService _priceHistoryService;

        public IntrinioPriceHistoryServiceTests(StocksFixture stocksFixture) : base(stocksFixture)
        {
            _priceHistoryService = new IntrinioPriceHistoryService(ServiceProvider.GetService<IIntrinioSecuritiesService>());
        }

        [Fact]
        public async Task CollectData()
        {
            var priceHistoryRequest = new PriceHistoryRequest("AAPL", new DateTime(2018, 1, 1), new DateTime(2019, 1, 1), FrequencyType.Daily);
            var serviceResponse = await _priceHistoryService.GetPriceHistoryAsync(priceHistoryRequest).ConfigureAwait(false);
            Assert.True(serviceResponse.IsSuccess, serviceResponse.ErrorMessage);
            Assert.NotNull(serviceResponse.Content);
            Assert.NotEmpty(serviceResponse.Content.CandleItems);
        }
    }
}
