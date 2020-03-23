using Stocks.Ameritrade.Models.PriceHistory;
using Stocks.Data.Entities;
using Stocks.Data.Repositories.Interfaces;
using Stocks.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Stocks.Ameritrade.Services.Interfaces;
using Moq;
using Stocks.Common.Models;
using Stocks.Models.Api;
using Stocks.Services.Collecting;
using Stocks.Intrinio.Services.Interfaces;

namespace Stocks.Tests.Services
{
    [Collection(Constants.CollectionName)]
    public class IntrinioCollectingServiceTests : BaseTests
    {
        private readonly ICollectingService _collectingService;
        private readonly IPriceHistoryRepository _priceHistoryRepository;

        public IntrinioCollectingServiceTests(StocksFixture stocksFixture) : base(stocksFixture)
        {
            _priceHistoryRepository = ServiceProvider.GetService<IPriceHistoryRepository>();
            _collectingService = new IntrinioCollectingService(ServiceProvider.GetService<IIntrinioSecuritiesService>(), _priceHistoryRepository);
        }

        [Fact]
        public async Task CollectData()
        {
            var priceHistoryRequest = new PriceHistoryRequest("AAPL", new DateTime(2018, 1, 1), new DateTime(2019, 1, 1), FrequencyType.Daily);
            var serviceResponse = await _collectingService.CollectDataAsync(priceHistoryRequest).ConfigureAwait(false);
            Assert.True(serviceResponse.IsSuccess, serviceResponse.ErrorMessage);

            PriceHistory priceHistory = await _priceHistoryRepository.GetAsync(priceHistoryRequest.Symbol).ConfigureAwait(false);
            Assert.NotNull(priceHistory);
            Assert.NotEmpty(priceHistory.Candles);
        }
    }
}
