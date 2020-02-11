using Stocks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stocks.Helpers
{
    public static class IndicatorHelper
    {
        public static void Calculate(List<ReportItem> reportItems)
        {
            reportItems[0].IndicatorItem = new IndicatorItem();
            for (int i = 1; i < reportItems.Count; i++)
            {
                var indicatorItem = new IndicatorItem();
                double? entryPrice = reportItems[i - 1].CandleItem.Close;
                double? atr = reportItems[i].AtrItem.ATR;
                indicatorItem.StopLoss = entryPrice - atr;
                indicatorItem.TakeProfit = entryPrice + atr * atr;

                double? price = reportItems[i].CandleItem.Close;
                if(price <= indicatorItem.StopLoss)
                {
                    indicatorItem.PriceResult = PriceResult.StopLossReached;
                }
                else if(price >= indicatorItem.TakeProfit)
                {
                    indicatorItem.PriceResult = PriceResult.TakeProfitReached;
                }

                reportItems[i].IndicatorItem = indicatorItem;
            }
        }
    }
}
