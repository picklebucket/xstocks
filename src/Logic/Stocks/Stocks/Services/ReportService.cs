using Stocks.Data.Entities;
using Stocks.Data.Repositories.Interfaces;
using Stocks.Helpers;
using Stocks.Models;
using Stocks.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Services
{
    public class ReportService : IReportService
    {
        private readonly IPriceHistoryRepository _priceHistoryRepository;

        public ReportService(IPriceHistoryRepository priceHistoryRepository)
        {
            _priceHistoryRepository = priceHistoryRepository;
        }

        public IEnumerable<ReportItem> BuildReport(string symbol, List<CandleItem> candleItems)
        {
            var reportItems = new List<ReportItem>();

            var ichimokuItems = IchimokuHelper.BuildIchimokuCloud(candleItems);
            var atrItems = AtrHelper.CalculateATR(candleItems);
            for (int i = 0; i < candleItems.Count; i++)
            {
                reportItems.Add(new ReportItem()
                {
                    CandleItem = candleItems[i],
                    IchimokuItem = ichimokuItems[i],
                    AtrItem = atrItems[i],
                    Symbol = symbol                    
                });
            }

            IndicatorHelper.Calculate(reportItems);

            return reportItems;
        }

        public async Task<List<CandleItem>> GetCollectedCandlesAsync(string symbol)
        {
            PriceHistory priceHistory = await _priceHistoryRepository.GetAsync(symbol);
            var candles = new List<CandleItem>();
            foreach (var candle in priceHistory.Candles)
            {
                candles.Add(new CandleItem()
                {
                    Date = DateTimeOffset.FromUnixTimeMilliseconds(candle.DateTimeUnixEpochMilliseconds),
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
