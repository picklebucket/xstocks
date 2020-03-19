using Intrinio.SDK.Model;
using Stocks.Common.Models;
using Stocks.Intrinio.Models;
using System;
namespace Stocks.Intrinio.Services.Interfaces
{
    public interface IIntrinioIchimokuService
    {
        ServiceResponse<ApiResponseSecurityIchimokuKinkoHyo> GetIchimoku(IchimokuRequest ichimokuRequest);
    }

}
