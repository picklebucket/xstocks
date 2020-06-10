using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Models
{
    public class CandleItem
    {
        public decimal? Open { get; set; }
        public decimal? Close { get; set; }
        public decimal? High { get; set; }
        public decimal? Low { get; set; }
        public DateTime? Date { get; set; }
        public decimal? Volume { get; set; }

        public CandleItem()
        {
        }

        public CandleItem(DateTime date)
        {
            Date = date;
        }
    }
}
