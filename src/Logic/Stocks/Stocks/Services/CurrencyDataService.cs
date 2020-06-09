using Stocks.Common.Models;
using Stocks.Data.Entities;
using Stocks.Data.Repositories.Interfaces;
using Stocks.Intrinio.Models.Currency;
using Stocks.Intrinio.Services.Interfaces;
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
    public class CurrencyDataService : ICurrencyDataService
    {
        private readonly IIntrinioForexCurrencyService _intrinioForexCurrencyService;
        private readonly ICurrencyDataRepository _currencyDataRepository;

        public CurrencyDataService(
            IIntrinioForexCurrencyService intrinioForexCurrencyService,
            ICurrencyDataRepository currencyDataRepository)
        {
            _intrinioForexCurrencyService = intrinioForexCurrencyService;
            _currencyDataRepository = currencyDataRepository;
        }

        public async Task<ServiceResponse<CurrencyData>> GetPairData(string pairData)
        {
            try
            {
                var data = await _currencyDataRepository.GetPairData(pairData).ConfigureAwait(false);
                return ServiceResponse<CurrencyData>.Success(data);
            }
            catch (Exception ex)
            {
                return ServiceResponse<CurrencyData>.Error(ex.Message);
            }
        }

        public async Task<ServiceResponse<CurrencyData>> SyncDataFor7days15minsInterval(string pairCode)
        {
            try
            {
                await _currencyDataRepository.WipeData(pairCode);

                var dateToday = DateTime.UtcNow.Date;
                var date14daysBefore = dateToday.AddDays(-14);
                ServiceResponse<CurrencyDataResponse> response = await _intrinioForexCurrencyService.GetCurrencyData(
                    new CurrencyDataRequest
                    {
                        PairCode = pairCode,
                        Timeframe = CurrencyPairTimeframeType.D1,
                        StartDate = date14daysBefore,
                        StartTime = date14daysBefore.ToString("HH:mm:ss"),
                        EndDate = dateToday,
                        EndTime = dateToday.ToString("HH:mm:ss"),
                        PageSize = 1000
                    }).ConfigureAwait(false);

                if (response.IsSuccess)
                {
                    var dbData = this.Convert(response.Content);
                    await _currencyDataRepository.CreateAsync(dbData).ConfigureAwait(false);
                    return ServiceResponse<CurrencyData>.Success(dbData);
                }

                return ServiceResponse<CurrencyData>.Error(response.ErrorMessage);
            }
            catch (Exception ex)
            {
                return ServiceResponse<CurrencyData>.Error(ex.Message);
            }
        }

        public async Task ReportToExcel(List<string> pairCodes)
        {
            var rocs = new List<CurrencyROCResult>();
            foreach (var pairCode in pairCodes)
            {
                var roc = await GetROC(pairCode).ConfigureAwait(false);
                rocs.Add(roc);
            }

            ReportToCsv(rocs);
        }

        public async Task<CurrencyROCResult> GetROCReport(string pairCode)
        {
            var roc = await GetROC(pairCode).ConfigureAwait(false);
            return roc;
        }

        private async Task<CurrencyROCResult> GetROC(string pairCode)
        {
            CurrencyData data = await _currencyDataRepository.GetPairData(pairCode).ConfigureAwait(false);
            return CalculateROC(data);
        }

        private CurrencyROCResult CalculateROC(CurrencyData data)
        {
            var result = new CurrencyROCResult()
            { 
                PairCode = data.PairCode 
            };

            for (var i = 0; i < data.Prices.Count; i++)
            {
                var entry = data.Prices[i];
                var model = new CurrencyPriceROC()
                {
                    CloseAsk = entry.CloseAsk,
                    CloseBid = entry.CloseBid,
                    OccurredAt = entry.OccurredAt
                };

                var oneWeekPeriodDate = entry.OccurredAt.AddDays(-6);
                var nineDaysPeriodDate = entry.OccurredAt.AddDays(-9);
                var currentClosePrice = (entry.CloseAsk + entry.CloseBid) / 2;
                for (int j = i + 1; j < data.Prices.Count; j++)
                {
                    if (data.Prices[j].OccurredAt <= oneWeekPeriodDate && model.Week1Rok == null)
                    {
                        var periodClosePrice = (data.Prices[j].CloseAsk + data.Prices[j].CloseBid) / 2;
                        model.Week1Rok = 100 * ((currentClosePrice - periodClosePrice) / periodClosePrice);
                    }

                    if (data.Prices[j].OccurredAt <= nineDaysPeriodDate && model.Week2Rok == null)
                    {
                        var periodClosePrice = (data.Prices[j].CloseAsk + data.Prices[j].CloseBid) / 2;
                        model.Week2Rok = 100 * ((currentClosePrice - periodClosePrice) / periodClosePrice);
                        break;
                    }
                }

                result.Data.Add(model);
            }

            return result;
        }

        private CurrencyData Convert(CurrencyDataResponse response)
        {
            var result = new CurrencyData();
            result.BaseCurrency = response.BaseCurrency;
            result.QuoteCurrency = response.QuoteCurrency;
            result.PairCode = response.CurrencyPairCode;
            result.Prices = response.Prices.Select(
                x => new CurrencyPrice
                {
                    CloseAsk = x.CloseAsk,
                    CloseBid = x.CloseBid,
                    HighAsk = x.HighAsk,
                    HighBid = x.HighBid,
                    LowAsk = x.LowAsk,
                    LowBid = x.LowBid,
                    OccurredAt = x.OccurredAt,
                    OpenAsk = x.OpenAsk,
                    OpenBid = x.OpenBid,
                    TotalTicks = x.TotalTicks
                }).ToList();

            return result;
        }

        private void ReportToCsv(List<CurrencyROCResult> rocs)
        {
            using var stream = new MemoryStream();
            using StreamWriter streamWriter = new StreamWriter(stream);
            foreach (var roc in rocs)
            {
                streamWriter.WriteLine(roc.PairCode);
                streamWriter.WriteLine("");

                streamWriter.WriteLine("Close Ask, Close Bid, Occurred At, 7 days ROC, 9 days ROC");
                foreach (var price in roc.Data)
                {
                    streamWriter.WriteLine($"{price.CloseAsk}, {price.CloseBid}, {price.OccurredAt}, {price.Week1Rok}, {price.Week2Rok}");
                }

                streamWriter.WriteLine("");
                streamWriter.WriteLine("");
            }

            streamWriter.Flush();
            byte[] content = stream.ToArray();
            string output = Encoding.Default.GetString(content);
            File.WriteAllText(@"d:\ROC.csv", output);
        }
    }
}
