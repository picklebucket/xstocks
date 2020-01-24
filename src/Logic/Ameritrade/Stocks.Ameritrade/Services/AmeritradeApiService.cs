using Stocks.Ameritrade.Models;
using Stocks.Ameritrade.Models.PriceHistory;
using Stocks.Ameritrade.Models.Token;
using Stocks.Ameritrade.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace Stocks.Ameritrade.Services
{
    public class AmeritradeApiService : IAmeritradeApiService
    {
        private static HttpClient _httpClient;
        
        private readonly string _refreshToken;
        private readonly string _clientId;

        private const string _tokenStorageFileName = "token.txt";
        private string _accessToken;
        private string AccessToken
        {
            get
            {
                if(_accessToken == null)
                {
                    if(File.Exists(_tokenStorageFileName))
                    {
                        _accessToken = File.ReadAllText(_tokenStorageFileName);
                    }
                }
                return _accessToken;
            }
            set
            {
                _accessToken = value;
                File.WriteAllText(_tokenStorageFileName, _accessToken);
            }
        }

        public AmeritradeApiService(ApiSettings apiSettings)
        {
            _refreshToken = apiSettings.RefreshToken;
            _clientId = apiSettings.ClientId;

            _httpClient = new HttpClient
            {
                BaseAddress = new Uri(apiSettings.BaseUrl)
            };
        }

        public async Task<ApiResponse<PriceHistoryResponse>> GetPriceHistory(string symbol, PriceHistoryRequest priceHistoryRequest)
        {
            if(string.IsNullOrEmpty(symbol))
            {
                throw new ArgumentException("Input cannot be null or empty", nameof(symbol));
            }
            if(priceHistoryRequest == null)
            {
                throw new ArgumentNullException(nameof(priceHistoryRequest));
            }

            var queryParameters = HttpUtility.ParseQueryString(string.Empty);
            if(priceHistoryRequest.PeriodType != PeriodType.Unspecified)
            {
                string periodType = string.Empty;
                switch (priceHistoryRequest.PeriodType)
                {
                    case PeriodType.Day:
                        periodType = "day";
                        break;
                    case PeriodType.Month:
                        periodType = "month";
                        break;
                    case PeriodType.Year:
                        periodType = "year";
                        break;
                    case PeriodType.Ytd:
                        periodType = "ytd";
                        break;
                }
                queryParameters.Add("periodType", periodType);
            }
            if(priceHistoryRequest.Period.HasValue)
            {
                queryParameters.Add("period", priceHistoryRequest.Period.Value.ToString());
            }
            if(priceHistoryRequest.FrequencyType != FrequencyType.Unspecified)
            {
                string frequencyType = string.Empty;
                switch (priceHistoryRequest.FrequencyType)
                {
                    case FrequencyType.Minute:
                        frequencyType = "minute";
                        break;
                    case FrequencyType.Daily:
                        frequencyType = "daily";
                        break;
                    case FrequencyType.Weekly:
                        frequencyType = "weekly";
                        break;
                    case FrequencyType.Monthly:
                        frequencyType = "monthly";
                        break;
                }
                queryParameters.Add("frequencyType", frequencyType);
            }

            var response = await GetAsync($"marketdata/{symbol}/pricehistory", queryParameters);
            string responseString = await response.Content.ReadAsStringAsync();
            if (response.IsSuccessStatusCode)
            {                
                PriceHistoryResponse priceHistory = JsonSerializer.Deserialize<PriceHistoryResponse>(responseString);
                return ApiResponse<PriceHistoryResponse>.Success(priceHistory);
            }
            else if(response.StatusCode == HttpStatusCode.BadRequest)
            {
                var error = JsonSerializer.Deserialize<BadRequestResponse>(responseString);
                return ApiResponse<PriceHistoryResponse>.Error(error.ErrorMessage);
            }

            return ApiResponse<PriceHistoryResponse>.Error("Unknown error");
        }

        private async Task<string> RefreshAccessTokenAsync()
        {
            var body = new Dictionary<string, string>
            {
                { "grant_type", "refresh_token" },
                { "client_id", _clientId },
                { "refresh_token", _refreshToken }
            };
            var response = await _httpClient.PostAsync("oauth2/token", new FormUrlEncodedContent(body));
            string accessToken = null;
            if(response.IsSuccessStatusCode)
            {
                string responseText = await response.Content.ReadAsStringAsync();
                var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseText);
                accessToken = tokenResponse.AccessToken;
            }
            return accessToken;
        }

        private async Task<HttpResponseMessage> GetAsync(string requestUri, NameValueCollection queryParameters)
        {
            string query = queryParameters.ToString();
            string url = requestUri;
            if(!string.IsNullOrEmpty(query))
            {
                url += $"?{query}";
            }
            return await SendAsync(() => new HttpRequestMessage(HttpMethod.Get, url));
        }

        private async Task<HttpResponseMessage> SendAsync(Func<HttpRequestMessage> requestMessageGetter)
        {
            var response = await SendAuthAsync(requestMessageGetter);
            //Unathorized error code could be because access token is expired. we will refresh it below
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                AccessToken = await RefreshAccessTokenAsync();
                response = await SendAuthAsync(requestMessageGetter);
            }
            return response;
        }

        private async Task<HttpResponseMessage> SendAuthAsync(Func<HttpRequestMessage> requestMessageGetter)
        {
            var requestMessage = requestMessageGetter.Invoke();
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            return await _httpClient.SendAsync(requestMessage);
        }
    }
}
