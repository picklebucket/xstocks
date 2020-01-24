using Stocks.Data.Entities;
using Stocks.Data.Repositories.Interfaces;
using Stocks.Data.Tests.Fixtures;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Stocks.Data.Tests.Repositories
{
    public class PriceHistoryRepositoryTests : IClassFixture<DataFixture>
    {
        private readonly IPriceHistoryRepository _priceHistoryRepository;

        public PriceHistoryRepositoryTests(DataFixture dataFixture)
        {
            _priceHistoryRepository = dataFixture.PriceHistoryRepository;
        }

        [Fact]
        public async Task Create()
        {
            var priceHistory = new PriceHistory()
            { 
                Symbol = "NFLX",
                Empty = true,
                Candles = new List<Candle>()
                {
                    new Candle()
                    {
                        Open = 1.1,
                        Close = 2.1,
                        DateTimeUnixEpochMilliseconds = 1231,
                        High = 1.3,
                        Low = 0.9,
                        Volume = 2
                    }
                }
            };

            await _priceHistoryRepository.CreateAsync(priceHistory);
            var createdPriceHistory = await _priceHistoryRepository.GetByIdAsync(priceHistory.Id);
            Assert.NotEmpty(createdPriceHistory.Candles);
            await _priceHistoryRepository.DeleteAsync(createdPriceHistory.Id);
        }
    }
}
