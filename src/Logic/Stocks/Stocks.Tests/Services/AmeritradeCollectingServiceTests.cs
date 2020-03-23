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

namespace Stocks.Tests.Services
{
    [Collection(Constants.CollectionName)]
    public class AmeritradeCollectingServiceTests : BaseTests
    {
        private readonly ICollectingService _collectingService;
        private readonly IPriceHistoryRepository _priceHistoryRepository;

        public AmeritradeCollectingServiceTests(StocksFixture stocksFixture) : base(stocksFixture)
        {
            var ameritradeApiServiceMock = new Mock<IAmeritradeApiService>();
            ameritradeApiServiceMock
                .Setup(x => x.GetPriceHistoryAsync(It.IsAny<string>(), It.IsAny<AmeritradePriceHistoryRequest>()))
                .ReturnsAsync(ServiceResponse<PriceHistoryResponse>.Success(
                    new PriceHistoryResponse()
                    {
                        Empty = false,
                        Candles = new List<CandleResponse>()
                        {
                            new CandleResponse()
                            {
                                High = 1
                            }
                        }
                    }));

            _priceHistoryRepository = ServiceProvider.GetService<IPriceHistoryRepository>();
            _collectingService = new AmeritradeCollectingService(ameritradeApiServiceMock.Object, _priceHistoryRepository);
            
        }

        [Fact]
        public async Task CollectData()
        {
            var priceHistoryRequest = new PriceHistoryRequest("NFLX", PeriodType.Year, 1, FrequencyType.Daily);
            await _collectingService.CollectDataAsync(priceHistoryRequest).ConfigureAwait(false);

            PriceHistory priceHistory = await _priceHistoryRepository.GetAsync(priceHistoryRequest.Symbol).ConfigureAwait(false);
            Assert.NotNull(priceHistory);
            Assert.NotEmpty(priceHistory.Candles);
        }
    }
}
