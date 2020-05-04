using Stocks.Common.Helpers;
using Stocks.Data.Entities;
using Stocks.Data.Repositories.Interfaces;
using Stocks.Helpers;
using Stocks.Models;
using Stocks.Models.Api;
using Stocks.Models.Api.Ichimoku;
using Stocks.Models.Api.PriceHistory;
using Stocks.Services.Collecting;
using Stocks.Services.Ichimoku;
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
        private readonly IIchimokuService _ichimokuService;
        private readonly IPriceHistoryService _priceHistoryService;

        public ReportService(IIchimokuService ichimokuService, IPriceHistoryService priceHistoryService)
        {
            _ichimokuService = ichimokuService;
            _priceHistoryService = priceHistoryService;
        }

        public async Task<IEnumerable<ReportItem>> BuildReportAsync(string symbol, DateTime startDate, DateTime endDate, FrequencyType frequencyType)
        {
            var ichimokuRequest = new IchimokuRequest(symbol, startDate, endDate, frequencyType);
            var ichimokuResponse = await _ichimokuService.BuildIchimokuCloudAsync(ichimokuRequest).ConfigureAwait(false);
            var ichimokuItems = ichimokuResponse.Content.Items;

            var priceHistoryRequest = new PriceHistoryRequest(symbol, startDate, endDate, frequencyType);
            var priceHistoryResponse = await _priceHistoryService.GetPriceHistoryAsync(priceHistoryRequest).ConfigureAwait(false);
            var candles = priceHistoryResponse.Content.CandleItems;

            var reportItems = BuildReport(symbol, ichimokuItems, candles);
            return reportItems;
        }

        public IEnumerable<ReportItem> BuildReport(string symbol, List<IchimokuItem> ichimokuItems, List<CandleItem> candleItems)
        {
            var reportItems = new List<ReportItem>();
            var signals = SignalHelper.GetSignals(ichimokuItems, candleItems);
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
            if (reportItems == null)
            {
                throw new ArgumentNullException(nameof(reportItems));
            }

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

            var groupedReports = reportItems.Where(x => x.SignalType != SignalType.None).GroupBy(x => x.Symbol);
            foreach (IGrouping<string, ReportItem> groupedReport in groupedReports)
            {
                string symbol = groupedReport.Key;
                int tradesTaken = groupedReport.Count();
                int numberOfLosses = 0;
                foreach (ReportItem reportItem in groupedReport)
                {
                    if((reportItem.IndicatorItem.PriceResult == PriceResult.StopLossReached) && 
                        (reportItem.SignalType == SignalType.BullishStrong || reportItem.SignalType == SignalType.BullishWeak))
                    {
                        numberOfLosses++;
                    }
                }
                int numberOfWins = tradesTaken - numberOfLosses;
                double winAverage = (numberOfWins / (double)tradesTaken) * 100;
                streamWriter.WriteLine($"{symbol}, {tradesTaken}, {numberOfLosses}, {numberOfWins}, {winAverage}");
            }
            streamWriter.Flush();
            byte[] content = stream.ToArray();
            return content;
        }        
    }
}
