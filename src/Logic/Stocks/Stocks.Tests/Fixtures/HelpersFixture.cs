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

namespace Stocks.Tests.Fixtures
{
    public class HelpersFixture
    {
        public IPriceHistoryRepository PriceHistoryRepository { get; }

        public HelpersFixture()
        {
            PriceHistoryRepository = new FilePriceHistoryRepository();
        }

    }

    public class FilePriceHistoryRepository : IPriceHistoryRepository
    {
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
            string json = await File.ReadAllTextAsync("priceHistory.json");
            var priceHistories = JsonSerializer.Deserialize<PriceHistory[]>(json);

            return priceHistories.FirstOrDefault(x => x.Symbol == symbol);
        }

        public Task<PriceHistory> GetByIdAsync(ObjectId id)
        {
            throw new NotImplementedException();
        }
    }
}
