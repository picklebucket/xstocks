using Intrinio.SDK.Model;
using Stocks.Common.Extensions;
using Stocks.Common.Models;
using Stocks.Intrinio.Models.Currency;
using Stocks.Intrinio.Services.Interfaces;
using Stocks.Intrinio.Wrappers;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Stocks.Intrinio.Services
{
    public class IntrinioForexCurrencyService : IIntrinioForexCurrencyService
    {
        private readonly IntrinioForexCurrencyApi _intrinioForexCurrencyApi;

        public IntrinioForexCurrencyService(IntrinioForexCurrencyApi intrinioForexCurrencyApi)
        {
            _intrinioForexCurrencyApi = intrinioForexCurrencyApi;
        }

        public async Task<ServiceResponse<CurrencyDataResponse>> GetCurrencyData(CurrencyDataRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            try
            {
                string timeFrame = GetTimeFrame(request.Timeframe);

                ApiResponseForexPrices currencyData = await _intrinioForexCurrencyApi.GetForexPricesAsync(request.PairCode, 
                    timeFrame, request.Timezone, request.StartDate, request.StartTime, request.EndDate, request.EndTime, 
                    request.PageSize).ConfigureAwait(false);

                CurrencyDataResponse result = Convert(currencyData);

                return ServiceResponse<CurrencyDataResponse>.Success(result);
            }
            catch (Exception ex)
            {
                return ServiceResponse<CurrencyDataResponse>.Error(ex.Message);
            }
        }

        private CurrencyDataResponse Convert(ApiResponseForexPrices apiResponse)
        {
            var result = new CurrencyDataResponse
            {
                CurrencyPairCode = apiResponse.Pair.Code,
                BaseCurrency = apiResponse.Pair.BaseCurrency,
                QuoteCurrency = apiResponse.Pair.QuoteCurrency,
                Prices = apiResponse.Prices.Select(
                    p => new PriceResponseData {
                        CloseAsk = p.CloseAsk.ToDecimal(),
                        CloseBid = p.CloseBid.ToDecimal(),
                        HighAsk = p.HighAsk.ToDecimal(),
                        HighBid = p.HighBid.ToDecimal(),
                        LowAsk = p.LowAsk.ToDecimal(),
                        LowBid = p.LowBid.ToDecimal(),
                        OccurredAt = p.OccurredAt.ConvertToDateTime(DateTime.MinValue),
                        OpenAsk = p.OpenAsk.ToDecimal(),
                        OpenBid = p.OpenBid.ToDecimal(),
                        TotalTicks = p.TotalTicks
                    }).ToList()
            };
            
            return result;
        }

        private string GetTimeFrame(CurrencyPairTimeframeType timeframeType)
        {
            switch (timeframeType)
            {
                case CurrencyPairTimeframeType.Min1:
                    return CurrencyPairTimeframe.Min1;
                case CurrencyPairTimeframeType.Min5:
                    return CurrencyPairTimeframe.Min5;
                case CurrencyPairTimeframeType.Min15:
                    return CurrencyPairTimeframe.Min15;
                case CurrencyPairTimeframeType.Min30:
                    return CurrencyPairTimeframe.Min30;

                case CurrencyPairTimeframeType.H1:
                    return CurrencyPairTimeframe.H1;
                case CurrencyPairTimeframeType.H2:
                    return CurrencyPairTimeframe.H2;
                case CurrencyPairTimeframeType.H3:
                    return CurrencyPairTimeframe.H3;
                case CurrencyPairTimeframeType.H4:
                    return CurrencyPairTimeframe.H4;
                case CurrencyPairTimeframeType.H6:
                    return CurrencyPairTimeframe.H6;
                case CurrencyPairTimeframeType.H8:
                    return CurrencyPairTimeframe.H8;

                case CurrencyPairTimeframeType.D1:
                    return CurrencyPairTimeframe.D1;

                case CurrencyPairTimeframeType.W1:
                    return CurrencyPairTimeframe.W1;

                case CurrencyPairTimeframeType.Month1:
                    return CurrencyPairTimeframe.Month1;

                default:
                    return CurrencyPairTimeframe.W1;
            }
        }
    }
}
