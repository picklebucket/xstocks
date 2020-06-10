using Stocks.Common.Models;
using Stocks.Data.Entities;
using Stocks.Data.Repositories.Interfaces;
using Stocks.Extensions;
using Stocks.Intrinio.Models.PriceHistory;
using Stocks.Intrinio.Services.Interfaces;
using Stocks.Models;
using Stocks.Models.Api;
using Stocks.Models.Api.PriceHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Services.Collecting
{
    public class IntrinioPriceHistoryService : IPriceHistoryService
    {
        private readonly IIntrinioSecuritiesService _intrinioSecuritiesService;

        public IntrinioPriceHistoryService(IIntrinioSecuritiesService intrinioSecuritiesService) 
        {
            _intrinioSecuritiesService = intrinioSecuritiesService;
        }

        public async Task<ServiceResponse<PriceHistoryResponse>> GetPriceHistoryAsync(PriceHistoryRequest priceHistoryRequest)
        {
            if (priceHistoryRequest == null)
            {
                throw new ArgumentNullException(nameof(priceHistoryRequest));
            }

            var request = new IntrinioPriceHistoryRequest(priceHistoryRequest.Symbol,
                priceHistoryRequest.StartDate, priceHistoryRequest.EndDate, priceHistoryRequest.FrequencyType.ToIntrinioFrequencyType());
            var response = await _intrinioSecuritiesService.GetStockPricesAsync(request).ConfigureAwait(false);
            if (response.IsSuccess)
            {
                string symbol = response.Content.Ticker;
                bool isEmpty = !response.Content.StockPrices.Any();
                var candles = response.Content.StockPrices.Select(x => new CandleItem()
                {
                    Open = x.Open,
                    Close = x.Close,
                    High = x.High,
                    Low = x.Low,
                    Volume = x.Volume,
                    Date = x.Date
                }).ToList();
                while(!string.IsNullOrEmpty(response.Content.NextPage))
                {
                    request.NextPage = response.Content.NextPage;
                    response = await _intrinioSecuritiesService.GetStockPricesAsync(request).ConfigureAwait(false);
                    if(response.IsSuccess)
                    {
                        candles.AddRange(response.Content.StockPrices.Select(x => new CandleItem()
                        {
                            Open = x.Open,
                            Close = x.Close,
                            High = x.High,
                            Low = x.Low,
                            Volume = x.Volume,
                            Date = x.Date
                        }));
                    }
                    else
                    {
                        break;
                    }
                }
                var priceHistory = new PriceHistoryResponse(symbol, isEmpty, candles);
                return ServiceResponse<PriceHistoryResponse>.Success(priceHistory);
            }
            else
            {
                return ServiceResponse<PriceHistoryResponse>.Error(response.ErrorMessage);
            }
        }
    }
}
