using Stocks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stocks.Helpers
{
    public static class IndicatorHelper
    {
        public static List<IndicatorItem> Calculate(List<AtrItem> atrItems, List<CandleItem> candleItems)
        {
            var indicatorItems = new List<IndicatorItem>()
            {
                new IndicatorItem()
            };
            for (int i = 1; i < atrItems.Count; i++)
            {
                double? atr = atrItems[i].ATR;
                var indicatorItem = new IndicatorItem
                {
                    EntryPrice = candleItems[i - 1].Close
                };
                indicatorItem.StopLoss = indicatorItem.EntryPrice - atr;
                indicatorItem.TakeProfit = indicatorItem.EntryPrice + atr * atr;

                double? price = candleItems[i].Close;
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
