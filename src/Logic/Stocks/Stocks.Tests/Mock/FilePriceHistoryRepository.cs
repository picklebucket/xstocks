using MongoDB.Bson;
using Stocks.Data.Entities;
using Stocks.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Stocks.Tests.Mock
{
    public class FilePriceHistoryRepository : IPriceHistoryRepository
    {
        public Task AddToSymbol(string symbol, List<Candle> candles)
        {
            throw new NotImplementedException();
        }

        public Task CreateAsync(PriceHistory entity)
        {
            throw new NotImplementedException();
        }

        public Task CreateManyAsync(params PriceHistory[] entities)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(string symbol)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(ObjectId id)
        {
            throw new NotImplementedException();
        }

        public async Task<PriceHistory> GetAsync(string symbol)
        {
            string json = await File.ReadAllTextAsync("priceHistory.json").ConfigureAwait(false);
            var priceHistories = JsonSerializer.Deserialize<PriceHistory[]>(json);

            return priceHistories.FirstOrDefault(x => x.Symbol == symbol);
        }

        public Task<PriceHistory> GetByIdAsync(ObjectId id)
        {
            throw new NotImplementedException();
        }
    }
}
