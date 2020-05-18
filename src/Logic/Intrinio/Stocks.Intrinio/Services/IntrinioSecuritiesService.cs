using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Intrinio.SDK.Model;
using Stocks.Common.Extensions;
using Stocks.Common.Helpers;
using Stocks.Common.Models;
using Stocks.Intrinio.Models;
using Stocks.Intrinio.Models.Ichimoku;
using Stocks.Intrinio.Models.PriceHistory;
using Stocks.Intrinio.Services.Interfaces;
using Stocks.Intrinio.Wrappers;

namespace Stocks.Intrinio.Services
{
    public class IntrinioSecuritiesService : IIntrinioSecuritiesService
    {
        private readonly IntrinioSecurityApi _intrinioSecurityApi;

        public IntrinioSecuritiesService(IntrinioSecurityApi intrinioSecurityApi)
        {
            _intrinioSecurityApi = intrinioSecurityApi;
        }
        public async Task<ServiceResponse<IntrinioPriceHistoryResponse>> GetStockPricesAsync(IntrinioPriceHistoryRequest priceHistoryRequest)
        {
            if(priceHistoryRequest == null)
            {
                throw new ArgumentNullException(nameof(priceHistoryRequest));
            }

            try
            {
                var frequency = priceHistoryRequest.Frequency switch
                {
                    IntrinioFrequencyType.Daily => "daily",
                    _ => null,
                };
                ApiResponseSecurityStockPrices apiResponse = await _intrinioSecurityApi
                    .GetSecurityStockPricesAsync(priceHistoryRequest.Identifier, priceHistoryRequest.StartDate,
                        priceHistoryRequest.EndDate, frequency, priceHistoryRequest.PageSize, priceHistoryRequest.NextPage)
                    .ConfigureAwait(false);
                var result = new IntrinioPriceHistoryResponse()
                {
                    Ticker = apiResponse.Security.Ticker,
                    StockPrices = apiResponse.StockPrices.Select(x => 
                        new PriceSummary() 
                        {
                            Open = x.Open,
                            Close = x.Close,
                            High = x.High,
                            Low = x.Low,
                            Volume = x.Volume,
                            DateTimeUnixEpochMilliseconds = DateHelper.GetDateTimeUnixEpochMilliseconds(x.Date)
                        }),
                    NextPage = apiResponse.NextPage
                };

                return ServiceResponse<IntrinioPriceHistoryResponse>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<IntrinioPriceHistoryResponse>.Error(e.Message);
            }
        }

        public async Task<ServiceResponse<IntrinioIchimokuResponse>> GetIchimokuAsync(IntrinioIchimokuRequest ichimokuRequest)
        {
            if (ichimokuRequest == null)
            {
                throw new ArgumentNullException(nameof(ichimokuRequest));
            }

            try
            {
                ApiResponseSecurityIchimokuKinkoHyo apiResponse = await _intrinioSecurityApi
                    .GetSecurityPriceTechnicalsIchimokuAsync(ichimokuRequest.Identifier, ichimokuRequest.LowPeriod, 
                        ichimokuRequest.MediumPeriod, ichimokuRequest.HighPeriod, ichimokuRequest.StartDate, ichimokuRequest.EndDate, 
                        ichimokuRequest.PageSize, ichimokuRequest.NextPage)
                    .ConfigureAwait(false);
                var result = new IntrinioIchimokuResponse()
                {
                    Technicals = apiResponse.Technicals.Select(x => new IchimokuTechnical()
                    {
                        ChikouSpan = x.ChikouSpan.ToDecimal(),
                        KijunSen = x.KijunSen.ToDecimal(),
                        SenkouSpanA = x.SenkouSpanA.ToDecimal(),
                        SenkouSpanB = x.SenkouSpanB.ToDecimal(),
                        TenkanSen = x.TenkanSen.ToDecimal(),
                        Date = DateHelper.GetDateTimeOffsetFromDateTime(x.DateTime)
                    }),
                    NextPage = apiResponse.NextPage
                };
                return ServiceResponse<IntrinioIchimokuResponse>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<IntrinioIchimokuResponse>.Error(e.Message);
            }
        }

        public async Task<ServiceResponse<RealtimePriceResponse>> GetRealtimeStockPriceAsync(string identifier)
        {
            try
            {
                RealtimeStockPrice intrinioResponse = await _intrinioSecurityApi.GetSecurityRealtimePriceAsync(identifier)
                    .ConfigureAwait(false);
                var result = new RealtimePriceResponse
                {
                    AskPrice = intrinioResponse.AskPrice,
                    AskSize = intrinioResponse.AskSize,
                    BidPrice = intrinioResponse.BidPrice,
                    BidSize = intrinioResponse.BidSize,
                    ExchangeVolume = intrinioResponse.ExchangeVolume,
                    HighPrice = intrinioResponse.HighPrice,
                    LastPrice = intrinioResponse.LastPrice,
                    LastTime = intrinioResponse.LastTime,
                    LowPrice = intrinioResponse.LowPrice,
                    MarketVolume = intrinioResponse.MarketVolume,
                    OpenPrice = intrinioResponse.OpenPrice,
                    Source = intrinioResponse.Source,
                    UpdatedOn = intrinioResponse.UpdatedOn
                };

                return ServiceResponse<RealtimePriceResponse>.Success(result);
            }
            catch (Exception ex)
            {
                return ServiceResponse<RealtimePriceResponse>.Error(ex.Message);
            }
        }
    }
}
