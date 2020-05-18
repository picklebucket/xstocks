using Stocks.Common.Models;
using Stocks.Data.Entities;
using Stocks.Data.Repositories.Interfaces;
using Stocks.Intrinio.Models.Currency;
using Stocks.Intrinio.Services.Interfaces;
using Stocks.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<ServiceResponse<List<CurrencyData>>> GetPairData(string pairData)
        {
            try
            {
                var data = await _currencyDataRepository.GetPairData(pairData).ConfigureAwait(false);
                return ServiceResponse<List<CurrencyData>>.Success(data);
            }
            catch (Exception ex)
            {
                return ServiceResponse<List<CurrencyData>>.Error(ex.Message);
            }
        }

        public async Task<ServiceResponse<CurrencyData>> SyncDataFor7days15minsInterval(string pairCode)
        {
            try
            {
                await _currencyDataRepository.WipeData(pairCode);

                var date14daysBefore = DateTime.UtcNow.AddDays(-14);
                var dateToday = DateTime.UtcNow;
                ServiceResponse<CurrencyDataResponse> response =
                    await _intrinioForexCurrencyService.GetCurrencyData(
                 new CurrencyDataRequest
                 {
                     PairCode = pairCode,
                     Timeframe = CurrencyPairTimeframeType.H1,
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
    }
}
