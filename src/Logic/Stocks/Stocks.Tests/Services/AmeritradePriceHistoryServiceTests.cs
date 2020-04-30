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
using Stocks.Services.Collecting;
using Stocks.Models.Api.PriceHistory;
using Stocks.Models.Api;

namespace Stocks.Tests.Services
{
    [Collection(Constants.CollectionName)]
    public class AmeritradePriceHistoryServiceTests : BaseTests
    {
        private readonly IPriceHistoryService _collectingService;

        public AmeritradePriceHistoryServiceTests(StocksFixture stocksFixture) : base(stocksFixture)
        {
            var ameritradeApiServiceMock = new Mock<IAmeritradeApiService>();
            ameritradeApiServiceMock
                .Setup(x => x.GetPriceHistoryAsync(It.IsAny<string>(), It.IsAny<AmeritradePriceHistoryRequest>()))
                .ReturnsAsync(ServiceResponse<AmeritradePriceHistoryResponse>.Success(
                    new AmeritradePriceHistoryResponse()
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

            _collectingService = new AmeritradePriceHistoryService(ameritradeApiServiceMock.Object);
            
        }

        [Fact]
        public async Task CollectData()
        {
            var priceHistoryRequest = new PriceHistoryRequest("NFLX", PeriodType.Year, 1, FrequencyType.Daily);
            var response = await _collectingService.GetPriceHistoryAsync(priceHistoryRequest).ConfigureAwait(false);
            Assert.True(response.IsSuccess);            
            Assert.NotNull(response.Content.CandleItems);
            Assert.NotEmpty(response.Content.CandleItems);
        }
    }
}
