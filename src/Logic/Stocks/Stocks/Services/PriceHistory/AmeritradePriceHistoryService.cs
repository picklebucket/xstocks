using Stocks.Ameritrade.Models.PriceHistory;
using Stocks.Ameritrade.Services.Interfaces;
using Stocks.Common.Models;
using Stocks.Data.Entities;
using Stocks.Data.Repositories.Interfaces;
using Stocks.Models;
using Stocks.Models.Api;
using Stocks.Models.Api.PriceHistory;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Stocks.Services.Collecting
{
    public class AmeritradePriceHistoryService : IPriceHistoryService
    {
        private readonly IAmeritradeApiService _ameritradeApiService;

        public AmeritradePriceHistoryService(IAmeritradeApiService ameritradeApiService)
        {
            _ameritradeApiService = ameritradeApiService;
        }

        public async Task<ServiceResponse<PriceHistoryResponse>> GetPriceHistoryAsync(PriceHistoryRequest priceHistoryRequest)
        {
            if (priceHistoryRequest == null)
            {
                throw new ArgumentNullException(nameof(priceHistoryRequest));
            }

            var response = await MakeRequestAsync(priceHistoryRequest).ConfigureAwait(false);
            if (response.IsSuccess)
            {
                var candles = response.Content.Candles.Select(x => new CandleItem()
                {
                    Open = x.Open,
                    Close = x.Close,
                    High = x.High,
                    Low = x.Low,
                    Volume = x.Volume,
                    Date = DateTimeOffset.FromUnixTimeMilliseconds(x.DateTimeUnixEpochMilliseconds)
                });
                var priceHistory = new PriceHistoryResponse(response.Content.Symbol, response.Content.Empty, candles.ToList());
                return ServiceResponse<PriceHistoryResponse>.Success(priceHistory);
            }
            else
            {
                return ServiceResponse<PriceHistoryResponse>.Error(response.ErrorMessage);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>")]
        protected async Task<ServiceResponse<AmeritradePriceHistoryResponse>> MakeRequestAsync(PriceHistoryRequest priceHistoryRequest)
        {
            var ameritradeRequest = new AmeritradePriceHistoryRequest()
            {
                FrequencyType = priceHistoryRequest.FrequencyType switch
                {
                    FrequencyType.Minute => AmeritradeFrequencyType.Minute,
                    FrequencyType.Daily => AmeritradeFrequencyType.Daily,
                    FrequencyType.Weekly => AmeritradeFrequencyType.Weekly,
                    FrequencyType.Monthly => AmeritradeFrequencyType.Monthly,
                    _ => AmeritradeFrequencyType.Unspecified
                },
                PeriodType = priceHistoryRequest.PeriodType switch
                {
                    PeriodType.Day => AmeritradePeriodType.Day,
                    PeriodType.Month => AmeritradePeriodType.Month,
                    PeriodType.Year => AmeritradePeriodType.Year,
                    PeriodType.Ytd => AmeritradePeriodType.Ytd,
                    _ => AmeritradePeriodType.Unspecified
                },
                Period = priceHistoryRequest.Period
            };
            var apiResponse = await _ameritradeApiService.GetPriceHistoryAsync(priceHistoryRequest.Symbol, ameritradeRequest).ConfigureAwait(false);
            return apiResponse;
        }       
    }
}
