using Stocks.Common.Models;
using Stocks.Data.Entities;
using Stocks.Data.Repositories.Interfaces;
using Stocks.Extensions;
using Stocks.Intrinio.Models.PriceHistory;
using Stocks.Intrinio.Services.Interfaces;
using Stocks.Models.Api;
using Stocks.Models.Api.PriceHistory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Services.Collecting
{
    public class IntrinioCollectingService : CollectingService<IntrinioPriceHistoryResponse>
    {
        private readonly IIntrinioSecuritiesService _intrinioSecuritiesService;

        public IntrinioCollectingService(IIntrinioSecuritiesService intrinioSecuritiesService, IPriceHistoryRepository priceHistoryRepository) 
            : base(priceHistoryRepository)
        {
            _intrinioSecuritiesService = intrinioSecuritiesService;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>")]
        protected override async Task<ServiceResponse<IntrinioPriceHistoryResponse>> GetPriceHistoryAsync(PriceHistoryRequest priceHistoryRequest)
        {
            var request = new IntrinioPriceHistoryRequest(priceHistoryRequest.Symbol, 
                priceHistoryRequest.StartDate, priceHistoryRequest.EndDate, priceHistoryRequest.FrequencyType.ToIntrinioFrequencyType());
            var response = await _intrinioSecuritiesService.GetStockPricesAsync(request).ConfigureAwait(false);
            return response;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>")]
        protected override PriceHistory MapToEntity(ServiceResponse<IntrinioPriceHistoryResponse> response)
        {
            var priceHistory = new PriceHistory()
            {
                Symbol = response.Content.Ticker,
                Empty = !response.Content.StockPrices.Any(),
                Candles = response.Content.StockPrices.Select(x => new Candle()
                {
                    Open = x.Open,
                    Close = x.Close,
                    High = x.High,
                    Low = x.Low,
                    Volume = x.Volume,
                    DateTimeUnixEpochMilliseconds = x.DateTimeUnixEpochMilliseconds
                }).ToList()
            };
            return priceHistory;
        }
    }
}
