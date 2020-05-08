using Stocks.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Helpers
{
    /// <summary>
    /// Calculates EMA (Exponential moving average)
    /// </summary>
    public static class EmaHelper
    {
        public static List<EmaItem> CalculateEma(List<CandleItem> candles, int periods = 50)
        {
            if(candles == null)
            {
                throw new ArgumentNullException(nameof(candles));
            }

            var emaList = new List<EmaItem>();
            if(candles.Count >= periods)
            {
                decimal multiplier = 2M / (periods + 1);
                decimal? sma = 0;//simple moving average
                for (int i = 0; i < periods - 1; i++)
                {
                    emaList.Add(new EmaItem(candles[i].Date));
                    sma += candles[i].Close / periods;
                }
                sma += candles[periods - 1].Close;
                emaList.Add(new EmaItem(candles[periods - 1].Date, sma));
                var emaPrev = sma;
                for (int i = periods - 1; i < candles.Count; i++)
                {
                    emaPrev = multiplier * (candles[i].Close - emaPrev) + emaPrev;
                    emaList.Add(new EmaItem(candles[i].Date, emaPrev));
                }
            }
            return emaList;
        }
    }
}
