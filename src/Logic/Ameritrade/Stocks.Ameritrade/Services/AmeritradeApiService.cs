using Stocks.Ameritrade.Models;
using Stocks.Ameritrade.Models.PriceHistory;
using Stocks.Ameritrade.Services.Interfaces;
using Stocks.Common.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
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

        public AmeritradeApiService(AmeritradeApiSettings apiSettings)
        {
            if (apiSettings == null)
            {
                throw new ArgumentNullException(nameof(apiSettings));
            }


            _refreshToken = apiSettings.RefreshToken;
            _clientId = apiSettings.ClientId;

            _httpClient = new HttpClient
            {
                BaseAddress = apiSettings.BaseUrl
            };
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "<Pending>")]
        public async Task<ServiceResponse<AmeritradePriceHistoryResponse>> GetPriceHistoryAsync(string symbol, AmeritradePriceHistoryRequest priceHistoryRequest)
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
            if(priceHistoryRequest.PeriodType != AmeritradePeriodType.Unspecified)
            {
                string periodType = string.Empty;
                switch (priceHistoryRequest.PeriodType)
                {
                    case AmeritradePeriodType.Day:
                        periodType = "day";
                        break;
                    case AmeritradePeriodType.Month:
                        periodType = "month";
                        break;
                    case AmeritradePeriodType.Year:
                        periodType = "year";
                        break;
                    case AmeritradePeriodType.Ytd:
                        periodType = "ytd";
                        break;
                }
                queryParameters.Add("periodType", periodType);
            }
            if(priceHistoryRequest.Period.HasValue)
            {
                queryParameters.Add("period", priceHistoryRequest.Period.Value.ToString(CultureInfo.InvariantCulture));
            }
            if(priceHistoryRequest.FrequencyType != AmeritradeFrequencyType.Unspecified)
            {
                string frequencyType = string.Empty;
                switch (priceHistoryRequest.FrequencyType)
                {
                    case AmeritradeFrequencyType.Minute:
                        frequencyType = "minute";
                        break;
                    case AmeritradeFrequencyType.Daily:
                        frequencyType = "daily";
                        break;
                    case AmeritradeFrequencyType.Weekly:
                        frequencyType = "weekly";
                        break;
                    case AmeritradeFrequencyType.Monthly:
                        frequencyType = "monthly";
                        break;
                }
                queryParameters.Add("frequencyType", frequencyType);
            }

            var response = await GetAsync($"marketdata/{symbol}/pricehistory", queryParameters).ConfigureAwait(false);
            string responseString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            if (response.IsSuccessStatusCode)
            {                
                AmeritradePriceHistoryResponse priceHistory = JsonSerializer.Deserialize<AmeritradePriceHistoryResponse>(responseString);
                return ServiceResponse<AmeritradePriceHistoryResponse>.Success(priceHistory);
            }
            else if(response.StatusCode == HttpStatusCode.BadRequest)
            {
                string errorMessage = GetErrorMessage(responseString);
                return ServiceResponse<AmeritradePriceHistoryResponse>.Error(errorMessage);
            }

            return ServiceResponse<AmeritradePriceHistoryResponse>.Error("Unknown error");
        }

        private string GetErrorMessage(string json)
        {
            return JsonDocument.Parse(json).RootElement.GetProperty("error").GetString();
        }

        private async Task<string> RefreshAccessTokenAsync()
        {
            var body = new Dictionary<string, string>
            {
                { "grant_type", "refresh_token" },
                { "client_id", _clientId },
                { "refresh_token", _refreshToken }
            };
            HttpResponseMessage response;
            using (var content = new FormUrlEncodedContent(body))
            {
                response = await _httpClient.PostAsync("oauth2/token", content).ConfigureAwait(false);
            }
            string accessToken = null;
            if(response.IsSuccessStatusCode)
            {
                string responseText = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                accessToken = JsonDocument.Parse(responseText).RootElement.GetProperty("access_token").GetString();
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
            return await SendAsync(() => new HttpRequestMessage(HttpMethod.Get, url)).ConfigureAwait(false);
        }

        private async Task<HttpResponseMessage> SendAsync(Func<HttpRequestMessage> requestMessageGetter)
        {
            var response = await SendAuthAsync(requestMessageGetter).ConfigureAwait(false);
            //Unathorized error code could be because access token is expired. we will refresh it below
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                AccessToken = await RefreshAccessTokenAsync().ConfigureAwait(false);
                response = await SendAuthAsync(requestMessageGetter).ConfigureAwait(false);
            }
            return response;
        }

        private async Task<HttpResponseMessage> SendAuthAsync(Func<HttpRequestMessage> requestMessageGetter)
        {
            var requestMessage = requestMessageGetter.Invoke();
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", AccessToken);
            return await _httpClient.SendAsync(requestMessage).ConfigureAwait(false);
        }
    }
}
