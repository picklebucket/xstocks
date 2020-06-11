using Stocks.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Helpers
{
    /// <summary>
    /// Calculates EMA (Exponential moving average)
    /// https://school.stockcharts.com/doku.php?id=technical_indicators:moving_averages
    /// </summary>
    public static class EmaHelper
    {
        public static List<SignalIndicatorItem> CalculateEma(List<CandleItem> candles, int periods = 50)
        {
            if(candles == null)
            {
                throw new ArgumentNullException(nameof(candles));
            }

            var emaList = new List<SignalIndicatorItem>();
            if(candles.Count >= periods)
            {
                //multiplier = 2/(N + 1) where N number of days in given period
                decimal multiplier = 2M / (periods + 1);

                //simple moving average SMA = (c1 + c2 + ... + cn)/n  
                decimal? sma = 0;
                for (int i = 0; i < periods - 1; i++)
                {
                    //add empty EMA for first N days
                    emaList.Add(new SignalIndicatorItem(candles[i].Date));
                    sma += candles[i].Close / periods;
                }
                sma += candles[periods - 1].Close / periods;

                //initial EMA is equal to SMA
                emaList.Add(new SignalIndicatorItem(candles[periods - 1].Date, sma));
                var emaPrev = sma;
                for (int i = periods; i < candles.Count; i++)
                {
                    //EMA = multiplier * (current close price - previous EMA) + previous EMA
                    emaPrev = multiplier * (candles[i].Close - emaPrev) + emaPrev;
                    emaList.Add(new SignalIndicatorItem(candles[i].Date, emaPrev));
                }
            }

            return emaList;
        }
    }
}
