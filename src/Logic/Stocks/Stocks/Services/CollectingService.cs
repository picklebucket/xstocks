using Stocks.Ameritrade.Models.PriceHistory;
using Stocks.Ameritrade.Services.Interfaces;
using Stocks.Data.Entities;
using Stocks.Data.Repositories.Interfaces;
using Stocks.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Services
{
    public class CollectingService : ICollectingService
    {
        private readonly IAmeritradeApiService _ameritradeApiService;
        private readonly IPriceHistoryRepository _priceHistoryRepository;

        public CollectingService(IAmeritradeApiService ameritradeApiService, IPriceHistoryRepository priceHistoryRepository)
        {
            _ameritradeApiService = ameritradeApiService;
            _priceHistoryRepository = priceHistoryRepository;
        }

        public async Task CollectDataAsync(string symbol, PriceHistoryRequest priceHistoryRequest)
        {
            var apiResponse = await _ameritradeApiService.GetPriceHistory(symbol, priceHistoryRequest);
            if(apiResponse.IsSuccess)
            {
                var priceHistory = new PriceHistory()
                {
                    Symbol = apiResponse.Content.Symbol,
                    Empty = apiResponse.Content.Empty,
                    Candles = apiResponse.Content.Candles.Select(x => new Data.Entities.Candle()
                    {
                        Open = x.Open,
                        Close = x.Close,
                        High = x.High,
                        Low = x.Low,
                        Volume = x.Volume,
                        DateTimeUnixEpochMilliseconds = x.DateTimeUnixEpochMilliseconds
                    }).ToList()
                };
                await _priceHistoryRepository.CreateAsync(priceHistory);
            }
        }
    }
}
