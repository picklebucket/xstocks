using Stocks.Common.Helpers;
using Stocks.Data.Entities;
using Stocks.Data.Repositories.Interfaces;
using Stocks.Models;
using Stocks.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Services
{
    public class PriceHistoryDataService : IPriceHistoryDataService
    {
        private readonly IPriceHistoryRepository _priceHistoryRepository;

        public PriceHistoryDataService(IPriceHistoryRepository priceHistoryRepository)
        {
            _priceHistoryRepository = priceHistoryRepository;
        }

        public async Task CreateAsync(string symbol, bool empty, IEnumerable<CandleItem> candles)
        {
            var priceHistory = new PriceHistory()
            {
                Symbol = symbol,
                Empty = empty,
                Candles = candles.Select(x => new Candle() 
                { 
                    Open = x.Open,
                    Close = x.Close,
                    High = x.High,
                    Low = x.Low,
                    Volume = x.Volume,
                    DateTimeUnixEpochMilliseconds = x.Date.HasValue ? (long?)x.Date.Value.ToUnixTimeMilliseconds() : null
                }).ToList()
            };
            await _priceHistoryRepository.CreateAsync(priceHistory).ConfigureAwait(false);
        }

        public async Task<List<CandleItem>> GetCandlesAsync(string symbol)
        {
            PriceHistory priceHistory = await _priceHistoryRepository.GetAsync(symbol).ConfigureAwait(false);
            var candles = new List<CandleItem>();
            foreach (var candle in priceHistory.Candles)
            {
                candles.Add(new CandleItem()
                {
                    Date = DateHelper.GetDateTimeOffsetFromUnixTimeMilliseconds(candle.DateTimeUnixEpochMilliseconds),
                    Open = candle.Open,
                    Close = candle.Close,
                    High = candle.High,
                    Low = candle.Low,
                    Volume = candle.Volume
                });
            }
            return candles;
        }
    }
}
