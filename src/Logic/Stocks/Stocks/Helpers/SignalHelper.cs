using Stocks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stocks.Helpers
{
    public static class SignalHelper
    {
        public static List<SignalType> GetSignals(List<IchimokuItem> ichimokuItems, List<CandleItem> candles, List<SignalIndicatorItem> emaItems)
        {
            if (ichimokuItems == null)
            {
                throw new ArgumentNullException(nameof(ichimokuItems));
            }
            if (candles == null)
            {
                throw new ArgumentNullException(nameof(candles));
            }

            var signalTypes = new List<SignalType>() { SignalType.None };
            for (int i = 1; i < candles.Count; i++)
            {
                SignalType signalType = CloudBreak(i, ichimokuItems, candles);
                if (signalType == SignalType.None)
                {
                    signalType = TrendCont(ichimokuItems[i + 26], candles[i]);
                    //if(signalType == SignalType.None)
                    //{
                    //    signalType = EmaBounce(i, candles, emaItems);
                    //}
                }
                signalTypes.Add(signalType);
            }
            return signalTypes;
        }

        #region Cloud break

        private static SignalType CloudBreak(int startIndex, List<IchimokuItem> ichimokuItems, List<CandleItem> candles)
        {
            int count = 0;
            int i = startIndex;
            if (IsCandleAboveCloud(candles[i], ichimokuItems[i + 26]) && IsCandleBelowCloud(candles[i - 1], ichimokuItems[i - 1 + 26]))
            {
                while (i < candles.Count && CanBeStrongBuySignal(ichimokuItems[i], ichimokuItems[i - 1], candles[i]))
                {
                    count++;
                    i++;
                    if (count == 3)
                    {
                        return SignalType.BullishStrong;
                    }
                }

            }
            //i = startIndex;
            //while (i < ichimokuItems.Count && CanBeWeakBuySignal(ichimokuItems[i], candles[i]))
            //{
            //    count++;
            //    i++;
            //    if (count == 3)
            //    {
            //        return SignalType.BullishWeak;
            //    }
            //}
            i = startIndex;
            if (IsCandleAboveCloud(candles[i - 1], ichimokuItems[i - 1 + 26]) && IsCandleBelowCloud(candles[i], ichimokuItems[i + 26]))
            {
                while (i < candles.Count && CanBeSellSignal(ichimokuItems[i], ichimokuItems[i - 1], candles[i]))
                {
                    count++;
                    i++;
                    if (count == 3)
                    {
                        return SignalType.BearishStrong;
                    }
                }
            }

            return SignalType.None;
        }

        private static bool IsCandleBelowCloud(CandleItem candleItem, IchimokuItem ichimokuItem)
        {
            return candleItem.Close < ichimokuItem.SenkouSpanA && candleItem.Close < ichimokuItem.SenkouSpanB;
        }

        private static bool IsCandleAboveCloud(CandleItem candleItem, IchimokuItem ichimokuItem)
        {
            return candleItem.Close > ichimokuItem.SenkouSpanA && candleItem.Close > ichimokuItem.SenkouSpanB;
        }

        private static bool CanBeStrongBuySignal(IchimokuItem ichimokuItem, IchimokuItem prevIchimokuItem, CandleItem candleItem)
        {
            //TODO: needs to check if Span A is above B like here, but also check if Span B is above Span A, so need an OR
            //Chikou Span (Lagging Span)
            /*
             cloud break out (bull example) : this is when the price breaks out above the cloud. it is considered:
             strong: 3 bars above the cloud, lagging line has popped out and is above the previous price 
             (you will need to see that visually to probably understand it)
		     
             avg: 3 bars but the lagging line has not cleared the cloud
		     weak: less than 3 bars have popped out of the cloud, i.e,. it is riding the line 
		     bear version is the same but opposite side of the cloud
            */
            return (ichimokuItem.ChikouSpan > ichimokuItem.SenkouSpanA && ichimokuItem.ChikouSpan > ichimokuItem.SenkouSpanB &&
                candleItem.Close > ichimokuItem.SenkouSpanA && candleItem.Close > ichimokuItem.SenkouSpanB &&
                prevIchimokuItem.ChikouSpan < prevIchimokuItem.SenkouSpanA && prevIchimokuItem.ChikouSpan < prevIchimokuItem.SenkouSpanB);
        }

        private static bool CanBeWeakBuySignal(IchimokuItem ichimokuItem, CandleItem candleItem)
        {
            return (ichimokuItem.SenkouSpanA > ichimokuItem.SenkouSpanB &&
                ichimokuItem.ChikouSpan <= ichimokuItem.SenkouSpanA &&
                candleItem.Low > ichimokuItem.SenkouSpanA)
                ||
                (ichimokuItem.SenkouSpanB > ichimokuItem.SenkouSpanA &&
                ichimokuItem.ChikouSpan <= ichimokuItem.SenkouSpanB &&
                candleItem.Low > ichimokuItem.SenkouSpanB);
        }

        private static bool CanBeSellSignal(IchimokuItem ichimokuItem, IchimokuItem prevIchimokuItem, CandleItem candleItem)
        {
            return (ichimokuItem.ChikouSpan < ichimokuItem.SenkouSpanA && ichimokuItem.ChikouSpan < ichimokuItem.SenkouSpanB &&
                candleItem.Close < ichimokuItem.SenkouSpanA && candleItem.Close < ichimokuItem.SenkouSpanB &&
                prevIchimokuItem.ChikouSpan > prevIchimokuItem.SenkouSpanA && prevIchimokuItem.ChikouSpan > prevIchimokuItem.SenkouSpanB);
        }

        #endregion

        #region Trend cont

        private static SignalType TrendCont(IchimokuItem ichimokuItem, CandleItem candleItem)
        {
            /*
             this is when the two lines, base and turning line (Tenkan Sen (Turning line) crosses the Kijun Sen (Standard line).)
             cross each other above the cloud with the price above the lines. 
             it is a strong signal when above the cloud, neutral signal in the cloud and weak below the cloud. 
             the bearish signal is the same but opposite */
            if ((ichimokuItem.SenkouSpanA > ichimokuItem.SenkouSpanB &&
                ichimokuItem.KijunSen > ichimokuItem.SenkouSpanA && ichimokuItem.TenkanSen > ichimokuItem.SenkouSpanA &&
                ichimokuItem.KijunSen < candleItem.Open && ichimokuItem.TenkanSen < candleItem.Open &&
                ichimokuItem.KijunSen < candleItem.Close && ichimokuItem.TenkanSen < candleItem.Close &&
                ichimokuItem.TenkanSen == ichimokuItem.KijunSen)
                ||
                (ichimokuItem.SenkouSpanA < ichimokuItem.SenkouSpanB &&
                ichimokuItem.KijunSen > ichimokuItem.SenkouSpanB && ichimokuItem.TenkanSen > ichimokuItem.SenkouSpanB &&
                ichimokuItem.KijunSen < candleItem.Open && ichimokuItem.TenkanSen < candleItem.Open &&
                ichimokuItem.KijunSen < candleItem.Close && ichimokuItem.TenkanSen < candleItem.Close &&
                ichimokuItem.TenkanSen == ichimokuItem.KijunSen))
            {
                return SignalType.BullishStrong;
            }
            if ((ichimokuItem.SenkouSpanA > ichimokuItem.SenkouSpanB &&
                ichimokuItem.KijunSen < ichimokuItem.SenkouSpanB && ichimokuItem.TenkanSen < ichimokuItem.SenkouSpanB &&
                ichimokuItem.KijunSen > candleItem.Open && ichimokuItem.TenkanSen > candleItem.Open &&
                ichimokuItem.KijunSen > candleItem.Close && ichimokuItem.TenkanSen > candleItem.Close &&
                ichimokuItem.TenkanSen == ichimokuItem.KijunSen)
                ||
                (ichimokuItem.SenkouSpanA < ichimokuItem.SenkouSpanB &&
                ichimokuItem.KijunSen < ichimokuItem.SenkouSpanA && ichimokuItem.TenkanSen < ichimokuItem.SenkouSpanA &&
                ichimokuItem.KijunSen > candleItem.Open && ichimokuItem.TenkanSen > candleItem.Open &&
                ichimokuItem.KijunSen > candleItem.Close && ichimokuItem.TenkanSen > candleItem.Close &&
                ichimokuItem.TenkanSen == ichimokuItem.KijunSen))
            {
                return SignalType.BearishStrong;
            }
            return SignalType.None;
        }

        #endregion

        #region Ema bounce

        private static SignalType EmaBounce(int index, List<CandleItem> candles, List<SignalIndicatorItem> emaItems)
        {
            SignalType signalType = SignalType.None;
            int prevIndex = index - 1;
            if (prevIndex > 0)
            {
                if ((candles[prevIndex].Low <= emaItems[prevIndex].Value && candles[prevIndex].High >= emaItems[prevIndex].Value)
                    && candles[index].Close > candles[prevIndex].Close)
                {
                    signalType = SignalType.BullishStrong;
                }
            }
            return signalType;
        }

        #endregion

    }
}
