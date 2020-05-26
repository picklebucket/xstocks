using Stocks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stocks.Helpers
{
    public static class AtrHelper
    {
        //https://school.stockcharts.com/doku.php?id=technical_indicators:average_true_range_atr
        //Typically, the Average True Range (ATR) is based on 14 periods and can be calculated on an intraday, daily, weekly or monthly basis.
        public static List<AtrItem> CalculateATR(List<CandleItem> candles)
        {
            if(candles == null)
            {
                throw new ArgumentNullException(nameof(candles));
            }

            var atrItems = new List<AtrItem>();
            for (int i = 0; i < candles.Count; i++)
            {
                var atrItem = new AtrItem()
                {
                    Date = candles[i].Date,
                    TR = CalculateTR(i, candles)
                };
                //skip first 13 days to accumulate data
                if (i >= 9)
                {
                    if(i == 9)
                    {
                        //get first 13 days
                        var trInput = atrItems.Take(9).ToList();
                        //add current 14th day
                        trInput.Add(atrItem);
                        //the first 14-day ATR is the average of the daily TR values for the last 14 days
                        atrItem.ATR = trInput.Average(x => x.TR);
                    }
                    else
                    {
                        //Current ATR = [(Prior ATR x 13) + Current TR] / 14
                        atrItem.ATR = (atrItems[i - 1].ATR * 9 + atrItem.TR) / 10;
                    }
                }
                atrItems.Add(atrItem);
            }

            return atrItems;
        }

        private static decimal? CalculateTR(int currentCandleIndex, List<CandleItem> candles)
        {
            var trInputData = new List<decimal?>();

            //current High - current Low
            decimal? currentHighCurrentLow = candles[currentCandleIndex].High - candles[currentCandleIndex].Low;
            trInputData.Add(currentHighCurrentLow);
            
            decimal? currentHighPreviousClose = null;
            decimal? currentLowPreviousClose = null;
            if(currentCandleIndex > 0)
            {
                var currentCandle = candles[currentCandleIndex];
                var previousCandle = candles[currentCandleIndex - 1];

                if(currentCandle.High.HasValue && previousCandle.Close.HasValue)
                {
                    //current High - previous Close
                    currentHighPreviousClose = Math.Abs((currentCandle.High - previousCandle.Close).Value);
                }
                trInputData.Add(currentHighPreviousClose);

                if (currentCandle.Low.HasValue && previousCandle.Close.HasValue)
                {
                    //current Low - previous Close
                    currentLowPreviousClose = Math.Abs((currentCandle.Low - previousCandle.Close).Value);
                }
                trInputData.Add(currentLowPreviousClose);
            }

            return trInputData.Max();
        }
    }
}
