using Stocks.Models;
using System;
using System.Collections.Generic;

namespace Stocks.Helpers
{
    public static class SmaHelper
    {
        public static List<SignalIndicatorItem> CalculateSma(List<CandleItem> candles, int periods = 50)
        {
            if (candles == null)
            {
                throw new ArgumentNullException(nameof(candles));
            }

            var smaList = new List<SignalIndicatorItem>();
            if (candles.Count >= periods)
            {
                //simple moving average SMA = (c1 + c2 + ... + cn) / n  
                decimal? sma = 0;
                for (int i = 0; i < periods - 1; i++)
                {
                    smaList.Add(new SignalIndicatorItem(candles[i].Date));
                    sma += candles[i].Close / periods;
                }

                sma += candles[periods - 1].Close / periods;
                smaList.Add(new SignalIndicatorItem(candles[periods - 1].Date, sma));

                for (int i = periods; i < candles.Count; i++)
                {
                    sma += candles[i].Close / periods;
                    sma -= candles[i-100].Close / periods;
                    smaList.Add(new SignalIndicatorItem(candles[i].Date, sma));
                }
            }

            return smaList;
        }
    }
}
