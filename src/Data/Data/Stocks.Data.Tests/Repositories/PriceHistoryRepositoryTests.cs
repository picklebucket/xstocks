using MongoDB.Bson;
using Stocks.Data.Entities;
using Stocks.Data.Repositories.Interfaces;
using Stocks.Data.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Stocks.Data.Tests.Repositories
{
    public class PriceHistoryRepositoryTests : IClassFixture<DataFixture>
    {
        private readonly IPriceHistoryRepository _priceHistoryRepository;
        private readonly PriceHistory _priceHistory;

        public PriceHistoryRepositoryTests(DataFixture dataFixture)
        {
            if(dataFixture == null)
            {
                throw new ArgumentNullException(nameof(dataFixture));
            }
            _priceHistoryRepository = dataFixture.PriceHistoryRepository;

            _priceHistory = new PriceHistory()
            {
                Symbol = "NFLX",
                Empty = true,
                Candles = new List<Candle>()
                {
                    new Candle()
                    {
                        Open = 1.1m,
                        Close = 2.1m,
                        DateTimeUnixEpochMilliseconds = 1231,
                        High = 1.3m,
                        Low = 0.9m,
                        Volume = 2
                    }
                }
            };

        }

        [Fact]
        public async Task Create()
        {
            await _priceHistoryRepository.CreateAsync(_priceHistory).ConfigureAwait(false);
            var createdPriceHistory = await _priceHistoryRepository.GetByIdAsync(_priceHistory.Id).ConfigureAwait(false);
            Assert.NotNull(createdPriceHistory.Candles);
            Assert.NotEmpty(createdPriceHistory.Candles);
            await _priceHistoryRepository.DeleteAsync(createdPriceHistory.Id).ConfigureAwait(false);
        }

        [Fact]
        public async Task GetById_NullResult()
        {
            var priceHistory = await _priceHistoryRepository.GetByIdAsync(ObjectId.Empty).ConfigureAwait(false);
            Assert.Null(priceHistory);
        }

        [Fact]
        public async Task GetBySymbol()
        {
            await _priceHistoryRepository.CreateAsync(_priceHistory).ConfigureAwait(false);
            var createdPriceHistory = await _priceHistoryRepository.GetAsync(_priceHistory.Symbol).ConfigureAwait(false);
            Assert.NotNull(createdPriceHistory);
        }

        [Fact]
        public async Task DeleteBySymbol()
        {
            await _priceHistoryRepository.CreateAsync(_priceHistory).ConfigureAwait(false);
            var createdPriceHistory = await _priceHistoryRepository.GetByIdAsync(_priceHistory.Id).ConfigureAwait(false);
            Assert.NotNull(createdPriceHistory);

            await _priceHistoryRepository.DeleteAsync(createdPriceHistory.Symbol).ConfigureAwait(false);
            var deletedPriceHistory = await _priceHistoryRepository.GetByIdAsync(createdPriceHistory.Id).ConfigureAwait(false);
            Assert.Null(deletedPriceHistory);
        }
    }
}
