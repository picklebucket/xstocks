using Stocks.Common.Models;
using Stocks.Models;
using Stocks.Models.Api.Ichimoku;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Services.Ichimoku
{
    public interface IIchimokuService
    {
        Task<ServiceResponse<IchimokuResponse>> BuildIchimokuCloudAsync(IchimokuRequest request);
    }
}
