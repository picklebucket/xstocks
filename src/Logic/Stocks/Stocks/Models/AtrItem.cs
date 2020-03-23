using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Models
{
    public class AtrItem
    {
        public DateTimeOffset? Date { get; set; }
        public decimal? TR { get; set; }
        public decimal? ATR { get; set; }
    }
}
