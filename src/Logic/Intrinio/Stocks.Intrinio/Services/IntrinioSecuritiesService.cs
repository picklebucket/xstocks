using System;
using System.Globalization;
using System.Threading.Tasks;
using Intrinio.SDK.Model;
using Stocks.Common.Models;
using Stocks.Intrinio.Models;
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
        public async Task<ServiceResponse<ApiResponseSecurityStockPrices>> GetStockPricesAsync(PriceHistoryRequest priceHistoryRequest)
        {
            if(priceHistoryRequest == null)
            {
                throw new ArgumentNullException(nameof(priceHistoryRequest));
            }

            try
            {
                var frequency = priceHistoryRequest.Frequency switch
                {
                    FrequencyType.Daily => "daily",
                    _ => null,
                };
                ApiResponseSecurityStockPrices result = await _intrinioSecurityApi
                    .GetSecurityStockPricesAsync(priceHistoryRequest.Identifier, priceHistoryRequest.StartDate,
                        priceHistoryRequest.EndDate, frequency, priceHistoryRequest.PageSize, priceHistoryRequest.NextPage)
                    .ConfigureAwait(false);

                return ServiceResponse<ApiResponseSecurityStockPrices>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<ApiResponseSecurityStockPrices>.Error(e.Message);
            }
        }

        public async Task<ServiceResponse<ApiResponseSecurityIchimokuKinkoHyo>> GetIchimokuAsync(IchimokuRequest ichimokuRequest)
        {
            if (ichimokuRequest == null)
            {
                throw new ArgumentNullException(nameof(ichimokuRequest));
            }

            try
            {
                ApiResponseSecurityIchimokuKinkoHyo result = await _intrinioSecurityApi
                    .GetSecurityPriceTechnicalsIchimokuAsync(ichimokuRequest.Identifier, ichimokuRequest.LowPeriod, 
                        ichimokuRequest.MediumPeriod, ichimokuRequest.HighPeriod, ichimokuRequest.StartDate, ichimokuRequest.EndDate, 
                        ichimokuRequest.PageSize, ichimokuRequest.NextPage)
                    .ConfigureAwait(false);
                return ServiceResponse<ApiResponseSecurityIchimokuKinkoHyo>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<ApiResponseSecurityIchimokuKinkoHyo>.Error(e.Message);
            }
        }
    }
}
