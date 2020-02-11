using Stocks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stocks.Helpers
{
    public static class AtrHelper
    {
        public static List<AtrItem> CalculateATR(List<CandleItem> candles)
        {
            var atrItems = new List<AtrItem>();
            for (int i = 0; i < candles.Count; i++)
            {
                var atrItem = new AtrItem()
                {
                    Date = candles[i].Date,
                    TR = CalculateTR(i, candles)
                };
                if(i >= 13)
                {
                    if(i == 13)
                    {
                        var trInput = atrItems.Take(13).ToList();
                        trInput.Add(atrItem);
                        atrItem.ATR = trInput.Average(x => x.TR);
                    }
                    else
                    {
                        atrItem.ATR = (atrItems[i - 1].ATR * 13 + atrItem.TR) / 14;
                    }
                }
                atrItems.Add(atrItem);
            }
            return atrItems;
        }

        private static double? CalculateTR(int currentCandleIndex, List<CandleItem> candles)
        {
            var trInputData = new List<double?>();

            double? currentHighCurrentLow = candles[currentCandleIndex].High - candles[currentCandleIndex].Low;
            trInputData.Add(currentHighCurrentLow);
            
            double? currentHighPreviousClose = null;
            double? currentLowPreviousClose = null;
            if(currentCandleIndex > 0)
            {
                var currentCandle = candles[currentCandleIndex];
                var previousCandle = candles[currentCandleIndex - 1];

                if(currentCandle.High.HasValue && previousCandle.Close.HasValue)
                {
                    currentHighPreviousClose = Math.Abs((currentCandle.High - previousCandle.Close).Value);
                }
                trInputData.Add(currentHighPreviousClose);

                if (currentCandle.Low.HasValue && previousCandle.Close.HasValue)
                {
                    currentLowPreviousClose = Math.Abs((currentCandle.Low - previousCandle.Close).Value);
                }
                trInputData.Add(currentLowPreviousClose);
            }

            return trInputData.Max();
        }
    }
}
