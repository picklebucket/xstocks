using Stocks.Common.Models;
using Stocks.Data.Entities;
using Stocks.Data.Repositories.Interfaces;
using Stocks.Models.Api.PriceHistory;
using System;
using System.Threading.Tasks;

namespace Stocks.Services.Collecting
{
    public abstract class CollectingService<TApiResponseContent> : ICollectingService
    {
        private readonly IPriceHistoryRepository _priceHistoryRepository;

        protected CollectingService(IPriceHistoryRepository priceHistoryRepository)
        {
            _priceHistoryRepository = priceHistoryRepository;
        }

        public async Task<ServiceResponse> CollectDataAsync(PriceHistoryRequest priceHistoryRequest)
        {
            if (priceHistoryRequest == null)
            {
                throw new ArgumentNullException(nameof(priceHistoryRequest));
            }

            var response = await GetPriceHistoryAsync(priceHistoryRequest).ConfigureAwait(false);
            if(response.IsSuccess)
            {
                var priceHistory = MapToEntity(response);
                await _priceHistoryRepository.CreateAsync(priceHistory).ConfigureAwait(false);
                return ServiceResponse.Success();
            }
            else
            {
                return ServiceResponse.Error(response.ErrorMessage);
            }
        }

        protected abstract Task<ServiceResponse<TApiResponseContent>> GetPriceHistoryAsync(PriceHistoryRequest priceHistoryRequest);

        protected abstract PriceHistory MapToEntity(ServiceResponse<TApiResponseContent> response);
    }
}
