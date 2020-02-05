using Stocks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stocks.Helpers
{
    public class AtrItem
    {
        public DateTimeOffset? Date { get; set; }
        public double? TR { get; set; }
        public double? ATR { get; set; } 
    }

    public static class AtrHelper
    {
        public static List<AtrItem> CalculateATR(IEnumerable<CandleModel> candles)
        {
            var atrItems = new List<AtrItem>();
            var candlesList = candles.ToList();
            for (int i = 0; i < candlesList.Count; i++)
            {
                var atrItem = new AtrItem()
                {
                    Date = candlesList[i].Date,
                    TR = CalculateTR(i, candlesList)
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

        private static double? CalculateTR(int currentCandleIndex, List<CandleModel> candles)
        {
            var trInputData = new List<double?>();

            double? currentHighCurrentLow = candles[currentCandleIndex].High - candles[currentCandleIndex].Low;
            trInputData.Add(currentHighCurrentLow);
            
            double? currentHighPreviousClose;
            double? currentLowPreviousClose;
            if(currentCandleIndex > 0)
            {
                currentHighPreviousClose = Math.Abs((candles[currentCandleIndex].High - candles[currentCandleIndex - 1].Close).Value);
                trInputData.Add(currentHighPreviousClose);

                currentLowPreviousClose = Math.Abs((candles[currentCandleIndex].Low - candles[currentCandleIndex - 1].Close).Value);
                trInputData.Add(currentLowPreviousClose);
            }
            
            return trInputData.Max();
        }
    }
}
