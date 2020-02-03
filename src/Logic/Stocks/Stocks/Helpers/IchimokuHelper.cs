using Stocks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stocks.Helpers
{
    public class IchimokuItem
    {
        public DateTimeOffset Date { get; set; }
        public double? TenkanSen { get; set; }
        public double? KijunSen { get; set; }
        public double? ChikouSpan { get; set; }
        public double? SenkouSpanA { get; set; }
        public double? SenkouSpanB { get; set; }

        public IchimokuItem()
        {
        }

        public IchimokuItem(DateTimeOffset date, double? tenkanSen, double? kijunSen, double? chikouSpan, double? senkouSpanA, double? senkouSpanB)
        {
            Date = date;
            TenkanSen = tenkanSen;
            KijunSen = kijunSen;
            ChikouSpan = chikouSpan;
            SenkouSpanA = senkouSpanA;
            SenkouSpanB = senkouSpanB;
        }
    }

    /// <summary>
    /// Builds Ichimoku cloud.
    /// See formulas in doc\ichimoku.xlsx 
    /// </summary>
    public static class IchimokuHelper
    {
        public const int SmallShiftSize = 8;
        public const int MediumShiftSize = 25;
        public const int LargeShiftSize = 51; 

        public static List<IchimokuItem> BuildIchimokuCloud(IEnumerable<CandleModel> candles)
        {
            var cloudItems = new List<IchimokuItem>();
            var extendedCandles = new List<CandleModel>(candles);
            int initialCandlesCount = extendedCandles.Count;
            if (initialCandlesCount > 0)
            {
                var lastCandle = candles.Last();
                DateTimeOffset dateTime = lastCandle.Date.Value;
                for (int i = 0; i < MediumShiftSize; i++)
                {
                    dateTime = dateTime.AddDays(1);
                    extendedCandles.Add(new CandleModel(dateTime));
                }

                for (int i = 0; i < extendedCandles.Count; i++)
                {
                    cloudItems.Add(new IchimokuItem()
                    {
                        Date = extendedCandles[i].Date.Value,
                        TenkanSen = GetTenkanSen(i, extendedCandles, initialCandlesCount),
                        KijunSen = GetKijunSen(i, extendedCandles, initialCandlesCount),
                        ChikouSpan = GetChikouSpan(i, extendedCandles),
                        SenkouSpanA = GetSenkouSpanA(i, cloudItems),
                        SenkouSpanB = GetSenkouSpanB(i, extendedCandles)
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
        private static double? GetTenkanSen(int currentCandleIndex, List<CandleModel> candles, int initialCandlesCount)
        {
            double? tenkanSen = null;
            if(currentCandleIndex < initialCandlesCount)
            {
                double? highestHigh = GetHighestHighForPeriod(currentCandleIndex, candles, SmallShiftSize);
                double? lowestLow = GetLowestLowForPeriod(currentCandleIndex, candles, SmallShiftSize);
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
        private static double? GetKijunSen(int currentCandleIndex, List<CandleModel> candles, int initialCandlesCount)
        {
            double? kijunSen = null;
            if (currentCandleIndex < initialCandlesCount)
            {
                double? highestHigh = GetHighestHighForPeriod(currentCandleIndex, candles, MediumShiftSize);
                double? lowestLow = GetLowestLowForPeriod(currentCandleIndex, candles, MediumShiftSize);
                if (highestHigh.HasValue && lowestLow.HasValue)
                {
                    kijunSen = (highestHigh.Value + lowestLow.Value) / 2;
                }
            }
            return kijunSen;
        }

        private static double? GetSenkouSpanA(int currentIndex, List<IchimokuItem> previousItems)
        {
            if(currentIndex >= 2 * MediumShiftSize)
            {
                int startIndex = currentIndex - MediumShiftSize + 1;
                var ichimokuItem = previousItems[startIndex];
                double? senkouSpanA = (ichimokuItem.TenkanSen + ichimokuItem.KijunSen) / 2;
                return senkouSpanA;
            }
            return null;
        }

        private static double? GetSenkouSpanB(int currentCandleIndex, List<CandleModel> candles)
        {
            if(currentCandleIndex >= LargeShiftSize + MediumShiftSize)
            {
                int startIndex = currentCandleIndex - MediumShiftSize + 1;
                double? highestHigh = GetHighestHighForPeriod(startIndex, candles, LargeShiftSize);
                double? lowestLow = GetLowestLowForPeriod(startIndex, candles, LargeShiftSize);
                if(highestHigh.HasValue && lowestLow.HasValue)
                {
                    double? senkouSpanB = (highestHigh + lowestLow) / 2;
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
        private static double? GetChikouSpan(int currentCandleIndex, List<CandleModel> candles)
        {
            int closePriceIndex = currentCandleIndex + MediumShiftSize;
            double? chikouSpan = null;
            if(candles.Count > closePriceIndex)
            {
                var candle = candles[closePriceIndex];
                if(candle.Close.HasValue)
                {
                    chikouSpan = candle.Close;
                }
            }
            return chikouSpan;
        }

        private static double? GetHighestHighForPeriod(int currentCandleIndex, List<CandleModel> candles, int shiftSize)
        {
            int indexDelta = shiftSize;
            if (currentCandleIndex >= indexDelta)
            {
                double? highestHigh = 0;
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

        private static double? GetLowestLowForPeriod(int currentCandleIndex, List<CandleModel> candles, int shiftSize)
        {
            int indexDelta = shiftSize;
            if (currentCandleIndex >= indexDelta)
            {
                double? lowestLow = double.MaxValue;
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
