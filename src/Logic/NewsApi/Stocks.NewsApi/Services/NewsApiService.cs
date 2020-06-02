using NewsAPI;
using NewsAPI.Models;
using Stocks.Common.Models;
using Stocks.NewsApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.NewsApi.Services
{
    public interface INewsApiService
    {
        Task<ServiceResponse<List<NewsDetailsModel>>> GetNews(string query, string country = null, int count = 10);
    }

    public class NewsApiService : INewsApiService
    {
        private readonly NewsApiClient _newsApiClient;

        public NewsApiService(NewsApiConfiguration apiConfiguration)
        {
            _newsApiClient = new NewsApiClient(apiConfiguration.GetKey());
        }

        public async Task<ServiceResponse<List<NewsDetailsModel>>> GetNews(string query, string country = null, int count = 10)
        {
            var request = new TopHeadlinesRequest() { Q = query };
            if (!string.IsNullOrEmpty(country))
            {
                NewsAPI.Constants.Countries countryParam = this.ResolveCountry(country);
                request.Country = countryParam;
            }

            var response = await _newsApiClient.GetTopHeadlinesAsync(request);
            if (response.Status == NewsAPI.Constants.Statuses.Error)
            {
                return ServiceResponse<List<NewsDetailsModel>>.Error($"{response.Error.Code} {response.Error.Message}");
            }

            return ServiceResponse<List<NewsDetailsModel>>.Success(
                response.Articles.Select(a => new NewsDetailsModel
                {
                    Title = a.Title,
                    Description = a.Description,
                    PublishedAt = a.PublishedAt
                }).ToList());
        }

        private NewsAPI.Constants.Countries ResolveCountry(string code)
        {
            switch (code)
            {
                case "us":
                    return NewsAPI.Constants.Countries.US;
                case "ru":
                    return NewsAPI.Constants.Countries.RU;
                default:
                    return NewsAPI.Constants.Countries.US;
            }
        }
    }
}
