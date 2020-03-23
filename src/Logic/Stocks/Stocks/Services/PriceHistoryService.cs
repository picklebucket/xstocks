using Stocks.Common.Helpers;
using Stocks.Data.Entities;
using Stocks.Data.Repositories.Interfaces;
using Stocks.Models;
using Stocks.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Services
{
    public class PriceHistoryService : IPriceHistoryService
    {
        private readonly IPriceHistoryRepository _priceHistoryRepository;

        public PriceHistoryService(IPriceHistoryRepository priceHistoryRepository)
        {
            _priceHistoryRepository = priceHistoryRepository;
        }

        public async Task<List<CandleItem>> GetCollectedCandlesAsync(string symbol)
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
