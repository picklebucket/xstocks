using Stocks.Common.Models;
using Stocks.Data.Entities;
using Stocks.Data.Repositories.Interfaces;
using Stocks.NewsApi.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Stocks.Services
{
    public class NewsDataService : INewsDataService
    {
        private readonly INewsApiService _newsApiService;
        private readonly INewsDetailsRepository _newsDetailsRepository;

        public NewsDataService(INewsApiService newsApiService, INewsDetailsRepository newsDetailsRepository)
        {
            this._newsApiService = newsApiService;
            this._newsDetailsRepository = newsDetailsRepository;
        }

        public async Task<ServiceResponse> SyncNews(string query)
        {
            try
            {
                var serviceResponse = await _newsApiService.GetNews(query).ConfigureAwait(false);
                if (serviceResponse.IsSuccess)
                {
                    var newsData = serviceResponse.Content.Select(n => new NewsDetails()
                    {
                        PublishedAt = n.PublishedAt,
                        Title = n.Title,
                        Text = n.Description
                    }).ToArray();

                    await _newsDetailsRepository.CreateManyAsync(newsData).ConfigureAwait(false);

                    return ServiceResponse.Success();
                }

                return ServiceResponse.Error(serviceResponse.ErrorMessage);
            }
            catch (Exception ex)
            {
                return ServiceResponse.Error(ex.Message);
            }
        }
    }
}
