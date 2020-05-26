using Stocks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stocks.Helpers
{
    public static class IndicatorHelper
    {
        public static List<IndicatorItem> Calculate(List<AtrItem> atrItems, List<CandleItem> candles, List<SignalType> signals)
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
                    Id = Guid.NewGuid(),
                    //magic 0.08
                    EntryPrice = candles[i - 1].Close + 0.08M
                };
                decimal? coefficient = 1.5M * atr;
                //SL = entry price - 1.5ATR
                indicatorItem.StopLoss = indicatorItem.EntryPrice - coefficient;
                //TP = entry price + 1.5ATR * 1.5ATR
                indicatorItem.TakeProfit = indicatorItem.EntryPrice + 2 * coefficient;
                indicatorItems.Add(indicatorItem);

                if (indicatorItem.StopLoss.HasValue && indicatorItem.TakeProfit.HasValue)
                {
                    for (var j = 1; j <= i; j++)
                    {
                        if (signals[j] != SignalType.None && indicatorItems[j].PriceResult == PriceResult.None)
                        {
                            if (candles[i].Low <= indicatorItems[j].StopLoss)
                            {
                                indicatorItems[j].PriceResult = PriceResult.StopLossReached;
                                indicatorItems[j].State = IndicatorState.Lose;
                                indicatorItems[j].ReachedDate = atrItems[i].Date;
                            }
                            else if (candles[i].High >= indicatorItems[j].TakeProfit)
                            {
                                indicatorItems[j].PriceResult = PriceResult.TakeProfitReached;
                                indicatorItems[j].State = IndicatorState.Win;
                                indicatorItems[j].ReachedDate = atrItems[i].Date;
                            }
                        }
                    }
                }                

            }

            return indicatorItems;
        }
    }
}
