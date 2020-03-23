using Stocks.Common.Helpers;
using Stocks.Common.Models;
using Stocks.Extensions;
using Stocks.Intrinio.Models.PriceHistory;
using Stocks.Intrinio.Services.Interfaces;
using Stocks.Models;
using Stocks.Models.Api.Ichimoku;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stocks.Services.Ichimoku
{
    /// <summary>
    /// Builds Ichimoku cloud.
    /// See formulas in doc\ichimoku.xlsx 
    /// </summary>
    public class ManualIchimokuService : IIchimokuService
    {
        private readonly IIntrinioSecuritiesService _intrinioSecuritiesService;

        public ManualIchimokuService(IIntrinioSecuritiesService intrinioSecuritiesService)
        {
            _intrinioSecuritiesService = intrinioSecuritiesService;
        }

        public async Task<ServiceResponse<IchimokuResponse>> BuildIchimokuCloudAsync(IchimokuRequest request)
        {
            if(request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            var intrinioPriceHistoryRequest = new IntrinioPriceHistoryRequest(request.Symbol, request.StartDate, 
                request.EndDate, request.Frequency.ToIntrinioFrequencyType());
            var response = await _intrinioSecuritiesService.GetStockPricesAsync(intrinioPriceHistoryRequest).ConfigureAwait(false);
            if(response.IsSuccess)
            {

                List<CandleItem> candles = response.Content.StockPrices.Select(x => new CandleItem() 
                {
                    Open = x.Open,
                    Close = x.Close,
                    Date = DateHelper.GetDateTimeOffsetFromUnixTimeMilliseconds(x.DateTimeUnixEpochMilliseconds),
                    High = x.High,
                    Low = x.Low,
                    Volume = x.Volume
                }).ToList();
                var ichimokuItems = BuildIchimokuCloud(candles);
                return ServiceResponse<IchimokuResponse>.Success(new IchimokuResponse(ichimokuItems));
            }
            return ServiceResponse<IchimokuResponse>.Error(response.ErrorMessage);
        }

        private List<IchimokuItem> BuildIchimokuCloud(List<CandleItem> candles)
        {
            if (candles == null)
            {
                throw new ArgumentNullException(nameof(candles));
            }

            var cloudItems = new List<IchimokuItem>();
            int initialCandlesCount = candles.Count;
            if (initialCandlesCount > 0)
            {
                var lastCandle = candles.Last();
                DateTimeOffset dateTime = lastCandle.Date.Value;
                for (int i = 0; i < IchimokuConstants.MediumShiftSize; i++)
                {
                    dateTime = dateTime.AddDays(1);
                    candles.Add(new CandleItem(dateTime));
                }

                for (int i = 0; i < candles.Count; i++)
                {
                    cloudItems.Add(new IchimokuItem()
                    {
                        Date = candles[i].Date.Value,
                        TenkanSen = GetTenkanSen(i, candles, initialCandlesCount),
                        KijunSen = GetKijunSen(i, candles, initialCandlesCount),
                        ChikouSpan = GetChikouSpan(i, candles),
                        SenkouSpanA = GetSenkouSpanA(i, cloudItems),
                        SenkouSpanB = GetSenkouSpanB(i, candles)
                    });
                }
            }
            return cloudItems;
        }

        /// <summary>
        /// 9 day average
        /// </summary>
        /// <param name="currentCandleIndex"></param>
        /// <param name="candles"></param>
        /// <returns></returns>
        private static decimal? GetTenkanSen(int currentCandleIndex, List<CandleItem> candles, int initialCandlesCount)
        {
            decimal? tenkanSen = null;
            if (currentCandleIndex < initialCandlesCount)
            {
                decimal? highestHigh = GetHighestHighForPeriod(currentCandleIndex, candles, IchimokuConstants.SmallShiftSize);
                decimal? lowestLow = GetLowestLowForPeriod(currentCandleIndex, candles, IchimokuConstants.SmallShiftSize);
                if (highestHigh.HasValue && lowestLow.HasValue)
                {
                    tenkanSen = (highestHigh.Value + lowestLow.Value) / 2;
                }
            }
            return tenkanSen;
        }


        /// <summary>
        /// 26 day average
        /// </summary>
        /// <param name="currentCandleIndex"></param>
        /// <param name="candles"></param>
        /// <returns></returns>
        private static decimal? GetKijunSen(int currentCandleIndex, List<CandleItem> candles, int initialCandlesCount)
        {
            decimal? kijunSen = null;
            if (currentCandleIndex < initialCandlesCount)
            {
                decimal? highestHigh = GetHighestHighForPeriod(currentCandleIndex, candles, IchimokuConstants.MediumShiftSize);
                decimal? lowestLow = GetLowestLowForPeriod(currentCandleIndex, candles, IchimokuConstants.MediumShiftSize);
                if (highestHigh.HasValue && lowestLow.HasValue)
                {
                    kijunSen = (highestHigh.Value + lowestLow.Value) / 2;
                }
            }
            return kijunSen;
        }

        private static decimal? GetSenkouSpanA(int currentIndex, List<IchimokuItem> previousItems)
        {
            if (currentIndex >= 2 * IchimokuConstants.MediumShiftSize)
            {
                int startIndex = currentIndex - IchimokuConstants.MediumShiftSize + 1;
                var ichimokuItem = previousItems[startIndex];
                decimal? senkouSpanA = (ichimokuItem.TenkanSen + ichimokuItem.KijunSen) / 2;
                return senkouSpanA;
            }
            return null;
        }

        private static decimal? GetSenkouSpanB(int currentCandleIndex, List<CandleItem> candles)
        {
            if (currentCandleIndex >= IchimokuConstants.LargeShiftSize + IchimokuConstants.MediumShiftSize)
            {
                int startIndex = currentCandleIndex - IchimokuConstants.MediumShiftSize + 1;
                decimal? highestHigh = GetHighestHighForPeriod(startIndex, candles, IchimokuConstants.LargeShiftSize);
                decimal? lowestLow = GetLowestLowForPeriod(startIndex, candles, IchimokuConstants.LargeShiftSize);
                if (highestHigh.HasValue && lowestLow.HasValue)
                {
                    decimal? senkouSpanB = (highestHigh + lowestLow) / 2;
                    return senkouSpanB;
                }
            }
            return null;
        }

        /// <summary>
        /// Lagging line (close price of +26 day)
        /// </summary>
        /// <param name="currentCandleIndex"></param>
        /// <param name="candles"></param>
        /// <returns></returns>
        private static decimal? GetChikouSpan(int currentCandleIndex, List<CandleItem> candles)
        {
            int closePriceIndex = currentCandleIndex + IchimokuConstants.MediumShiftSize;
            decimal? chikouSpan = null;
            if (candles.Count > closePriceIndex)
            {
                var candle = candles[closePriceIndex];
                if (candle.Close.HasValue)
                {
                    chikouSpan = candle.Close;
                }
            }
            return chikouSpan;
        }

        private static decimal? GetHighestHighForPeriod(int currentCandleIndex, List<CandleItem> candles, int shiftSize)
        {
            int indexDelta = shiftSize;
            if (currentCandleIndex >= indexDelta)
            {
                decimal? highestHigh = 0;
                int startIndex = currentCandleIndex - indexDelta;
                int endIndex = currentCandleIndex;
                for (int i = startIndex; i <= endIndex; i++)
                {
                    if (candles[i].High > highestHigh)
                    {
                        highestHigh = candles[i].High;
                    }
                }
                return highestHigh;
            }
            return null;
        }

        private static decimal? GetLowestLowForPeriod(int currentCandleIndex, List<CandleItem> candles, int shiftSize)
        {
            int indexDelta = shiftSize;
            if (currentCandleIndex >= indexDelta)
            {
                decimal? lowestLow = decimal.MaxValue;
                int startIndex = currentCandleIndex - indexDelta;
                int endIndex = currentCandleIndex;
                for (int i = startIndex; i <= endIndex; i++)
                {
                    if (candles[i].Low < lowestLow)
                    {
                        lowestLow = candles[i].Low;
                    }
                }
                return lowestLow;
            }
            return null;
        }


    }
}
