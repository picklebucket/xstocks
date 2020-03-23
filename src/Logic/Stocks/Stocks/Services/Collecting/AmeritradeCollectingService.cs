using Stocks.Ameritrade.Models.PriceHistory;
using Stocks.Ameritrade.Services.Interfaces;
using Stocks.Common.Models;
using Stocks.Data.Entities;
using Stocks.Data.Repositories.Interfaces;
using Stocks.Models.Api;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Stocks.Services.Collecting
{
    public class AmeritradeCollectingService : CollectingService<PriceHistoryResponse>
    {
        private readonly IAmeritradeApiService _ameritradeApiService;

        public AmeritradeCollectingService(IAmeritradeApiService ameritradeApiService, IPriceHistoryRepository priceHistoryRepository)
            : base(priceHistoryRepository)
        {
            _ameritradeApiService = ameritradeApiService;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>")]
        protected override async Task<ServiceResponse<PriceHistoryResponse>> GetPriceHistoryAsync(PriceHistoryRequest priceHistoryRequest)
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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "<Pending>")]
        protected override PriceHistory MapToEntity(ServiceResponse<PriceHistoryResponse> response)
        {
            var priceHistory = new PriceHistory()
            {
                Symbol = response.Content.Symbol,
                Empty = response.Content.Empty,
                Candles = response.Content.Candles.Select(x => new Candle()
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
