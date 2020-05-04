using Stocks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stocks.Helpers
{
    public static class SignalHelper
    {
        public static List<SignalType> GetSignals(List<IchimokuItem> ichimokuItems, List<CandleItem> candles)
        {
            if (ichimokuItems == null)
            {
                throw new ArgumentNullException(nameof(ichimokuItems));
            }
            if (candles == null)
            {
                throw new ArgumentNullException(nameof(candles));
            }

            var signalTypes = new List<SignalType>();
            for (int i = 0; i < ichimokuItems.Count; i++)
            {
                SignalType signalType = CloudBreak(i, ichimokuItems, candles);  
                if(signalType == SignalType.None)
                {
                    signalType = TrendCont(ichimokuItems[i], candles[i]);
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
            while (i < ichimokuItems.Count && CanBeStrongBuySignal(ichimokuItems[i], candles[i]))
            {
                count++;
                i++;
                if (count == 3)
                {
                    return SignalType.BullishStrong;
                }
            }
            i = startIndex;
            while (i < ichimokuItems.Count && CanBeWeakBuySignal(ichimokuItems[i], candles[i]))
            {
                count++;
                i++;
                if (count == 3)
                {
                    return SignalType.BullishWeak;
                }
            }
            i = startIndex;
            while (i < ichimokuItems.Count && CanBeSellSignal(ichimokuItems[i], candles[i]))
            {
                count++;
                i++;
                if (count == 3)
                {
                    return SignalType.Bearish;
                }
            }
            return SignalType.None;
        }

        private static bool CanBeStrongBuySignal(IchimokuItem ichimokuItem, CandleItem candleItem)
        {
            //TODO: needs to check if Span A is above B like here, but also check if Span B is above Span A, so need an OR

            return (ichimokuItem.SenkouSpanA > ichimokuItem.SenkouSpanB &&
                ichimokuItem.ChikouSpan > ichimokuItem.SenkouSpanA && 
                candleItem.Low > ichimokuItem.SenkouSpanA) 
                ||
                (ichimokuItem.SenkouSpanB > ichimokuItem.SenkouSpanA &&
                ichimokuItem.ChikouSpan > ichimokuItem.SenkouSpanB &&
                candleItem.Low > ichimokuItem.SenkouSpanB);
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

        private static bool CanBeSellSignal(IchimokuItem ichimokuItem, CandleItem candleItem)
        {
            return (ichimokuItem.SenkouSpanA > ichimokuItem.SenkouSpanB &&
                candleItem.High < ichimokuItem.SenkouSpanB)
                ||
                (ichimokuItem.SenkouSpanB > ichimokuItem.SenkouSpanA &&
                candleItem.High < ichimokuItem.SenkouSpanA);
        }

        #endregion

        #region Trend cont

        private static SignalType TrendCont(IchimokuItem ichimokuItem, CandleItem candleItem)
        {
            if((ichimokuItem.SenkouSpanA > ichimokuItem.SenkouSpanB &&
                ichimokuItem.KijunSen >= ichimokuItem.SenkouSpanA && ichimokuItem.TenkanSen >= ichimokuItem.SenkouSpanA &&
                ichimokuItem.KijunSen < candleItem.Low && ichimokuItem.TenkanSen < candleItem.Low)
                ||
                (ichimokuItem.SenkouSpanA < ichimokuItem.SenkouSpanB &&
                ichimokuItem.KijunSen >= ichimokuItem.SenkouSpanB && ichimokuItem.TenkanSen >= ichimokuItem.SenkouSpanB &&
                ichimokuItem.KijunSen < candleItem.Low && ichimokuItem.TenkanSen < candleItem.Low))
            {
                return SignalType.BullishStrong;
            }
            if ((ichimokuItem.SenkouSpanA > ichimokuItem.SenkouSpanB &&
                ichimokuItem.KijunSen <= ichimokuItem.SenkouSpanB && ichimokuItem.TenkanSen <= ichimokuItem.SenkouSpanB &&
                ichimokuItem.KijunSen > candleItem.High && ichimokuItem.TenkanSen > candleItem.High)
                ||
                (ichimokuItem.SenkouSpanA < ichimokuItem.SenkouSpanB &&
                ichimokuItem.KijunSen <= ichimokuItem.SenkouSpanA && ichimokuItem.TenkanSen <= ichimokuItem.SenkouSpanA &&
                ichimokuItem.KijunSen > candleItem.High && ichimokuItem.TenkanSen > candleItem.High))
            {
                return SignalType.Bearish;
            }
            return SignalType.None;
        }

        #endregion
    }
}
