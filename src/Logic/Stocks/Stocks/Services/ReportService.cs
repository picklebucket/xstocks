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
        private readonly IPriceHistoryDataService _priceHistoryDataService;

        private readonly IGoodPriceCompaniesRepository _goodPriceCompaniesRepository;

        public ReportService(IIchimokuService ichimokuService, IPriceHistoryService priceHistoryService,
            IPriceHistoryDataService priceHistoryDataService, IGoodPriceCompaniesRepository goodPriceCompaniesRepository)
        {
            _ichimokuService = ichimokuService;
            _priceHistoryService = priceHistoryService;
            _goodPriceCompaniesRepository = goodPriceCompaniesRepository;
            _priceHistoryDataService = priceHistoryDataService;
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

            //data comes in descending order
            candleItems.Reverse();
            ichimokuItems.Reverse();

            var emaItems = EmaHelper.CalculateEma(candleItems);
            var atrItems = AtrHelper.CalculateATR(candleItems);
            var signals = SignalHelper.GetSignals(ichimokuItems, candleItems, emaItems);
            var indicatorItems = IndicatorHelper.Calculate(atrItems, candleItems, signals);
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

        private async Task<List<CandleItem>> GetPriceHistory(string symbol, DateTime startDate, DateTime endDate, FrequencyType frequencyType)
        {
            var result = await this._priceHistoryDataService.GetCandlesAsync(symbol, startDate, endDate).ConfigureAwait(false);
            if (result != null && result.Count > 0)
            {
                DateTime lastSyncedDate = result.LastOrDefault().Date.Value.Date.AddDays(1);
                DateTime firstSyncedDate = result.FirstOrDefault().Date.Value.Date.AddDays(-1);
                if (lastSyncedDate < endDate)
                {
                    var priceHistoryRequest = new PriceHistoryRequest(symbol, lastSyncedDate, endDate, frequencyType);
                    var priceHistoryResponse = await _priceHistoryService.GetPriceHistoryAsync(priceHistoryRequest).ConfigureAwait(false);

                    var responseCandles = priceHistoryResponse.Content.CandleItems;
                    responseCandles.Reverse();

                    await this._priceHistoryDataService.AddToSymbol(symbol, responseCandles).ConfigureAwait(false);
                    result.AddRange(responseCandles);
                } 

                if (firstSyncedDate > startDate)
                {
                    var priceHistoryRequest = new PriceHistoryRequest(symbol, startDate, firstSyncedDate, frequencyType);
                    var priceHistoryResponse = await _priceHistoryService.GetPriceHistoryAsync(priceHistoryRequest).ConfigureAwait(false);

                    var responseCandles = priceHistoryResponse.Content.CandleItems;
                    responseCandles.Reverse();

                    await this._priceHistoryDataService.AddToSymbol(symbol, responseCandles).ConfigureAwait(false);
                    result.AddRange(responseCandles);
                }

                result = result.OrderBy(x => x.Date).ToList();
            }
            else
            {
                var priceHistoryRequest = new PriceHistoryRequest(symbol, startDate, endDate, frequencyType);
                var priceHistoryResponse = await _priceHistoryService.GetPriceHistoryAsync(priceHistoryRequest).ConfigureAwait(false);

                var responseCandles = priceHistoryResponse.Content.CandleItems;
                responseCandles.Reverse();

                await this._priceHistoryDataService.CreateAsync(symbol, false, responseCandles).ConfigureAwait(false);
                result = responseCandles;
            }

            return result;
        }

        public async Task<List<Ema50ReportItem>> Build50EmaBullReportAsync(string symbol, DateTime startDate, DateTime endDate, FrequencyType frequencyType)
        {
            var candleItems = await GetPriceHistory(symbol, startDate, endDate, frequencyType).ConfigureAwait(false);

            var result = new List<Ema50ReportItem>();
            var emaItems = EmaHelper.CalculateEma(candleItems);
            int possiblySignle = -1;
            for (int i = 0; i < candleItems.Count; i++)
            {
                var ema = emaItems[i];
                var candle = candleItems[i];
                var reportItem = new Ema50ReportItem
                {
                    Open = candle.Open,
                    Close = candle.Close,
                    Date = candle.Date.Value,
                    Ticks = (Int32)(candle.Date.Value.Subtract(new DateTime(1970, 1, 1))).TotalSeconds,
                    Ema = ema.Ema.GetValueOrDefault(0)
                };
                
                if (possiblySignle != -1)
                {
                    if (ema.Ema >= candle.Close)
                    {
                        result[possiblySignle].IsEma50 = true;
                    }

                    possiblySignle = -1;
                }

                if ((ema.Ema >= candle.Open && ema.Ema <= candle.Close) || (ema.Ema >= candle.Close && ema.Ema <= candle.Open))
                {
                    possiblySignle = i;
                }

                result.Add(reportItem);
            }

            return result;
        }

        public async Task<List<Ema50ReportItem>> Build50EmaBearReportAsync(string symbol, DateTime startDate, DateTime endDate, FrequencyType frequencyType)
        {
            var candleItems = await GetPriceHistory(symbol, startDate, endDate, frequencyType).ConfigureAwait(false);

            var result = new List<Ema50ReportItem>();
            var emaItems = EmaHelper.CalculateEma(candleItems);
            int possiblySignle = -1;
            for (int i = 0; i < candleItems.Count; i++)
            {
                var ema = emaItems[i];
                var candle = candleItems[i];
                var reportItem = new Ema50ReportItem
                {
                    Open = candle.Open,
                    Close = candle.Close,
                    Date = candle.Date.Value,
                    Ticks = (Int32)(candle.Date.Value.Subtract(new DateTime(1970, 1, 1))).TotalSeconds,
                    Ema = ema.Ema.GetValueOrDefault(0)
                };

                if (possiblySignle != -1)
                {
                    if (ema.Ema < candle.Close)
                    {
                        result[possiblySignle].IsEma50 = true;
                    }

                    possiblySignle = -1;
                }

                if ((ema.Ema >= candle.Open && ema.Ema <= candle.Close) || (ema.Ema >= candle.Close && ema.Ema <= candle.Open))
                {
                    possiblySignle = i;
                }

                result.Add(reportItem);
            }

            return result;
        }

        public async Task Build50EmaCSVReportAsync(string symbol, DateTime startDate, DateTime endDate, FrequencyType frequencyType)
        {
            var data = await this.Build50EmaBullReportAsync(symbol, startDate, endDate, frequencyType).ConfigureAwait(false);
            using var stream = new MemoryStream();
            using StreamWriter streamWriter = new StreamWriter(stream);
            streamWriter.WriteLine("Candle Open, Candle Close, Date, EMA, EMA Signle");
            foreach (var reportItem in data)
            {
                streamWriter.WriteLine($"{reportItem.Open}, {reportItem.Close}, {reportItem.Date}, {reportItem.Ema}, {(reportItem.IsEma50 ? "Signal" : "")}");
            }

            streamWriter.Flush();
            byte[] content = stream.ToArray();
            string output = Encoding.Default.GetString(content);
            File.WriteAllText(@"d:\ema50.csv", output);
        }

        public async Task<IReadOnlyCollection<GoodPriceCompany>> BuildReportBasedOnIntrinsicValue(DateTime? startDate = null, DateTime? endDate = null)
        {
            var result = await _goodPriceCompaniesRepository.GetForPeriod(startDate, endDate).ConfigureAwait(false);
            return result;
        }

        public byte[] ExportGoodPriceCompaniesToCsv(IReadOnlyCollection<GoodPriceCompany> goodPriceCompanies)
        {
            if (goodPriceCompanies == null)
            {
                throw new ArgumentNullException(nameof(goodPriceCompanies));
            }

            using var stream = new MemoryStream();
            using StreamWriter streamWriter = new StreamWriter(stream);
            streamWriter.WriteLine("Symbol, Intrinsic Value, Last Trade Price, Last Trade Time, Analysis Date");
            foreach (var reportItem in goodPriceCompanies)
            {
                streamWriter.WriteLine($"{reportItem.Symbol}, {reportItem.IntrinsicValue}, {reportItem.LastTradePrice}, {reportItem.LastTradeTime}, {reportItem.DateTime}");
            }
            streamWriter.Flush();
            byte[] content = stream.ToArray();
            return content;
        }

        public byte[] ExportToIndicatorCsv(IEnumerable<ReportItem> reportItems)
        {
            if (reportItems == null)
            {
                throw new ArgumentNullException(nameof(reportItems));
            }

            using var stream = new MemoryStream();
            using StreamWriter streamWriter = new StreamWriter(stream);
            streamWriter.WriteLine("Symbol, Date, Signal, Candle Low, Candle High, Entry Price, Stop Loss, Take Profit, ATR, Hit SL or TP, State, Reached Date");
            var previousSignal = SignalType.None;
            foreach (var reportItem in reportItems)
            {
                //if ((reportItem.SignalType != previousSignal && reportItem.SignalType != SignalType.None) || reportItem.IndicatorItem.PriceResult != PriceResult.None)
                if (reportItem.IndicatorItem.PriceResult != PriceResult.None && reportItem.SignalType != SignalType.None)
                {
                    streamWriter.WriteLine($"{reportItem.Symbol}, {reportItem.CandleItem.Date}, {reportItem.SignalType}, " +
                        $"{reportItem.CandleItem.Low}, {reportItem.CandleItem.High}, {reportItem.IndicatorItem.EntryPrice}, " +
                        $"{reportItem.IndicatorItem.StopLoss}, {reportItem.IndicatorItem.TakeProfit}, {reportItem.AtrItem.ATR}, " +
                        $"{reportItem.IndicatorItem.PriceResult}, {reportItem.IndicatorItem.State}, " +
                        $"{reportItem.IndicatorItem.ReachedDate}");
                    previousSignal = reportItem.SignalType;
                }
            }
            streamWriter.WriteLine("");
            streamWriter.WriteLine("");
            streamWriter.WriteLine("Symbol, Trades taken, Number of losses, Number of wins, Average win %");
            var groupedReports = reportItems.Where(x => x.SignalType != SignalType.None).GroupBy(x => x.Symbol);
            foreach (IGrouping<string, ReportItem> groupedReport in groupedReports)
            {
                string symbol = groupedReport.Key;
                int tradesTaken = groupedReport.Count();
                int numberOfLosses = 0;
                foreach (ReportItem reportItem in groupedReport)
                {
                    if ((reportItem.IndicatorItem.PriceResult == PriceResult.StopLossReached) &&
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
                    //if ((reportItem.IndicatorItem.PriceResult == PriceResult.StopLossReached) &&
                    //    (reportItem.SignalType == SignalType.BullishStrong || reportItem.SignalType == SignalType.BullishWeak))
                    if (reportItem.IndicatorItem.State == IndicatorState.Lose)
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
