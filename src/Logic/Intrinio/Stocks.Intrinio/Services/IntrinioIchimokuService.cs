using System;
using Intrinio.SDK.Model;
using Stocks.Common.Models;
using Stocks.Intrinio.Models;
using Stocks.Intrinio.Services.Interfaces;
using Stocks.Intrinio.Wrappers;

namespace Stocks.Intrinio.Services
{
    public class IntrinioIchimokuService : IIntrinioIchimokuService
    {
        private readonly IntrinioSecurityApi _securityApi;
        public IntrinioIchimokuService(IntrinioSecurityApi securityApi)
        {
            _securityApi = securityApi;
        }
        public ServiceResponse<ApiResponseSecurityIchimokuKinkoHyo> GetIchimoku(IchimokuRequest ichimokuRequest)
        {
            try
            {
                ApiResponseSecurityIchimokuKinkoHyo result = _securityApi
                    .GetSecurityPriceTechnicalsIchimoku(ichimokuRequest.Identifier, ichimokuRequest.LowPeriod, ichimokuRequest.MediumPeriod,
                    ichimokuRequest.HighPeriod, ichimokuRequest.StartDate, ichimokuRequest.EndDate, ichimokuRequest.PageSize, ichimokuRequest.NextPage);
                return ServiceResponse<ApiResponseSecurityIchimokuKinkoHyo>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<ApiResponseSecurityIchimokuKinkoHyo>.Error(e.Message);
            }
        }

    }
}
