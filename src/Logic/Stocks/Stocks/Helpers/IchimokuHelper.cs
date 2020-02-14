using Stocks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stocks.Helpers
{
    /// <summary>
    /// Builds Ichimoku cloud.
    /// See formulas in doc\ichimoku.xlsx 
    /// </summary>
    public static class IchimokuHelper
    {
        public const int SmallShiftSize = 8;
        public const int MediumShiftSize = 25;
        public const int LargeShiftSize = 51; 

        public static List<IchimokuItem> BuildIchimokuCloud(List<CandleItem> candles)
        {
            var cloudItems = new List<IchimokuItem>();
            int initialCandlesCount = candles.Count;
            if (initialCandlesCount > 0)
            {
                var lastCandle = candles.Last();
                DateTimeOffset dateTime = lastCandle.Date.Value;
                for (int i = 0; i < MediumShiftSize; i++)
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

        public static List<SignalType> GetSignals(List<IchimokuItem> ichimokuItems, List<CandleItem> candleItems)
        {
            var signalTypes = new List<SignalType>();
            for (int i = 0; i < ichimokuItems.Count; i++)
            {
                SignalType signalType = SignalType.None;
                int count = 0;
                int j = i;
                while(CanBeBuySignal(ichimokuItems[j], candleItems[j]))
                {
                    count++;
                    j++;
                    if(count == 3)
                    {
                        signalType = SignalType.BuySignal;
                    }
                }
                signalTypes.Add(signalType);
            }
            return signalTypes;
        }

        private static bool CanBeBuySignal(IchimokuItem ichimokuItem, CandleItem candleItem)
        {
            return ichimokuItem.ChikouSpan > ichimokuItem.SenkouSpanA && candleItem.Close > ichimokuItem.SenkouSpanA && ichimokuItem.SenkouSpanA > ichimokuItem.SenkouSpanB;
        }

        /// <summary>
        /// 9 day average
        /// </summary>
        /// <param name="currentCandleIndex"></param>
        /// <param name="candles"></param>
        /// <returns></returns>
        private static double? GetTenkanSen(int currentCandleIndex, List<CandleItem> candles, int initialCandlesCount)
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
        private static double? GetKijunSen(int currentCandleIndex, List<CandleItem> candles, int initialCandlesCount)
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

        private static double? GetSenkouSpanB(int currentCandleIndex, List<CandleItem> candles)
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
        private static double? GetChikouSpan(int currentCandleIndex, List<CandleItem> candles)
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

        private static double? GetHighestHighForPeriod(int currentCandleIndex, List<CandleItem> candles, int shiftSize)
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

        private static double? GetLowestLowForPeriod(int currentCandleIndex, List<CandleItem> candles, int shiftSize)
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
