using Stocks.Common.Models;
using Stocks.Intrinio.Models.Ichimoku;
using Stocks.Intrinio.Services.Interfaces;
using Stocks.Models;
using Stocks.Models.Api.Ichimoku;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Services.Ichimoku
{
    public class IntrinioIchimokuService : IIchimokuService
    {
        private readonly IIntrinioSecuritiesService _intrinioSecuritiesService;

        public IntrinioIchimokuService(IIntrinioSecuritiesService intrinioSecuritiesService)
        {
            _intrinioSecuritiesService = intrinioSecuritiesService;
        }

        public async Task<ServiceResponse<IchimokuResponse>> BuildIchimokuCloudAsync(IchimokuRequest request)
        {
            if(request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var intrinioIchimokuRequest = new IntrinioIchimokuRequest(request.Symbol, request.StartDate, request.EndDate);
            List<IchimokuItem> items = new List<IchimokuItem>();
            do
            {
                var response = await _intrinioSecuritiesService.GetIchimokuAsync(intrinioIchimokuRequest).ConfigureAwait(false);
                if (response.IsSuccess)
                {
                    items.AddRange(response.Content.Technicals.Select(x => new IchimokuItem()
                    {
                        Date = x.Date,
                        ChikouSpan = x.ChikouSpan,
                        KijunSen = x.KijunSen,
                        SenkouSpanA = x.SenkouSpanA,
                        SenkouSpanB = x.SenkouSpanB,
                        TenkanSen = x.TenkanSen
                    })); 
                    if(response.Content.NextPage != null)
                    {
                        intrinioIchimokuRequest.NextPage = response.Content.NextPage;
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    return ServiceResponse<IchimokuResponse>.Error(response.ErrorMessage);
                }
            } while (true);
            return ServiceResponse<IchimokuResponse>.Success(new IchimokuResponse(items));            
        }
    }
}
