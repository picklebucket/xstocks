using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Models
{
    public enum PriceResult
    {
        None = 0,
        StopLossReached = -1,
        TakeProfitReached = 2
    }
}
