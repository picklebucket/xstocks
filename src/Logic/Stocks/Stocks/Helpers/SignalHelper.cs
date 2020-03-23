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
                SignalType signalType = SignalType.None;
                int count = 0;
                int j = i;
                while(CanBeBuySignal(ichimokuItems[j], candles[j]))
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
            //TODO: needs to check if Span A is above B like here, but also check if Span B is above Span A, so need an OR

            return ichimokuItem.ChikouSpan > ichimokuItem.SenkouSpanA && candleItem.Close > ichimokuItem.SenkouSpanA && ichimokuItem.SenkouSpanA > ichimokuItem.SenkouSpanB;
        }
    }
}
