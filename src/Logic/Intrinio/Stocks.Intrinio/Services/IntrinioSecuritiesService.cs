using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Intrinio.SDK.Model;
using Stocks.Common.Helpers;
using Stocks.Common.Models;
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
                        ChikouSpan = ToDecimal(x.ChikouSpan),
                        KijunSen = ToDecimal(x.KijunSen),
                        SenkouSpanA = ToDecimal(x.SenkouSpanA),
                        SenkouSpanB = ToDecimal(x.SenkouSpanB),
                        TenkanSen = ToDecimal(x.TenkanSen),
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

        private decimal? ToDecimal(float? number)
        {
            if(number.HasValue)
            {
                return Convert.ToDecimal(number.Value, CultureInfo.InvariantCulture);
            }
            return null;
        }
    }
}
