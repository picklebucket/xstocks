using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Models
{
    public class EmaItem
    {
        public DateTimeOffset? Date { get; set; }
        public decimal? Ema { get; set; }

        public EmaItem(DateTimeOffset? date)
        {
            Date = date;
        }

        public EmaItem(DateTimeOffset? date, decimal? ema)
            : this(date)
        {
            Ema = ema;
        }
    }
}
