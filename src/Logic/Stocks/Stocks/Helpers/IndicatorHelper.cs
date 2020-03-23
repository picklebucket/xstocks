using Stocks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stocks.Helpers
{
    public static class IndicatorHelper
    {
        public static List<IndicatorItem> Calculate(List<AtrItem> atrItems, List<CandleItem> candles)
        {
            if (atrItems == null)
            {
                throw new ArgumentNullException(nameof(atrItems));
            }
            if (candles == null)
            {
                throw new ArgumentNullException(nameof(candles));
            }


            var indicatorItems = new List<IndicatorItem>()
            {
                new IndicatorItem()
            };
            for (int i = 1; i < atrItems.Count; i++)
            {
                decimal? atr = atrItems[i].ATR;
                var indicatorItem = new IndicatorItem
                {
                    EntryPrice = candles[i - 1].Close
                };
                indicatorItem.StopLoss = indicatorItem.EntryPrice - atr;
                indicatorItem.TakeProfit = indicatorItem.EntryPrice + atr * atr;

                decimal? price = candles[i].Close;
                if(price <= indicatorItem.StopLoss)
                {
                    indicatorItem.PriceResult = PriceResult.StopLossReached;
                }
                else if(price >= indicatorItem.TakeProfit)
                {
                    indicatorItem.PriceResult = PriceResult.TakeProfitReached;
                }

                indicatorItems.Add(indicatorItem);
            }
            return indicatorItems;
        }
    }
}
