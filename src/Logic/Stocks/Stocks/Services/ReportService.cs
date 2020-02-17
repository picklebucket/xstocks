using Stocks.Data.Entities;
using Stocks.Data.Repositories.Interfaces;
using Stocks.Helpers;
using Stocks.Models;
using Stocks.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            var signals = IchimokuHelper.GetSignals(ichimokuItems, candleItems);
            var atrItems = AtrHelper.CalculateATR(candleItems);
            var indicatorItems = IndicatorHelper.Calculate(atrItems, candleItems);
            for (int i = 0; i < candleItems.Count; i++)
            {
                reportItems.Add(new ReportItem()
                {
                    CandleItem = candleItems[i],
                    IchimokuItem = ichimokuItems[i],
                    AtrItem = atrItems[i],
                    IndicatorItem = indicatorItems[i],
                    Symbol = symbol,
                    SignalType = signals[i]
                });
            }            

            return reportItems;
        }

        public byte[] ExportToIndicatorCsv(IEnumerable<ReportItem> reportItems)
        {
            using var stream = new MemoryStream();
            using StreamWriter streamWriter = new StreamWriter(stream);
            streamWriter.WriteLine("Symbol, Date, Signal, Entry Price, Stop Loss, Take Profit, Result");
            foreach (var reportItem in reportItems)
            {
                streamWriter.WriteLine($"{reportItem.Symbol}, {reportItem.CandleItem.Date}, {reportItem.SignalType}, {reportItem.IndicatorItem.EntryPrice}, {reportItem.IndicatorItem.StopLoss}, {reportItem.IndicatorItem.TakeProfit}, {reportItem.IndicatorItem.PriceResult}");
            }
            streamWriter.Flush();
            byte[] content = stream.ToArray();
            return content;
        }

        public byte[] ExportToTradeCsv(IEnumerable<ReportItem> reportItems)
        {
            using var stream = new MemoryStream();
            using StreamWriter streamWriter = new StreamWriter(stream);
            streamWriter.WriteLine("Symbol, Trades taken, Number of losses, Number of wins, Average win %");
            
            var groupedReports = reportItems.Where(x => x.SignalType == SignalType.BuySignal).GroupBy(x => x.Symbol);
            foreach (IGrouping<string, ReportItem> groupedReport in groupedReports)
            {
                string symbol = groupedReport.Key;
                int tradesTaken = groupedReport.Count();
                int numberOfLosses = groupedReport.Count(x => x.IndicatorItem.PriceResult == PriceResult.StopLossReached);
                int numberOfWins = tradesTaken - numberOfLosses;
                double winAverage = (numberOfWins / (double)tradesTaken) * 100;
                streamWriter.WriteLine($"{symbol}, {tradesTaken}, {numberOfLosses}, {numberOfWins}, {winAverage}");
            }
            streamWriter.Flush();
            byte[] content = stream.ToArray();
            return content;
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
