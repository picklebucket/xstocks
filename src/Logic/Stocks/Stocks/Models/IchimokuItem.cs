using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Models
{
    public class IchimokuItem
    {
        public DateTimeOffset Date { get; set; }
        public decimal? TenkanSen { get; set; }
        public decimal? KijunSen { get; set; }
        /// <summary>
        /// Lagging line
        /// </summary>
        public decimal? ChikouSpan { get; set; }
        public decimal? SenkouSpanA { get; set; }
        public decimal? SenkouSpanB { get; set; }

        public IchimokuItem()
        {
        }

        public IchimokuItem(DateTimeOffset date, decimal? tenkanSen, decimal? kijunSen, decimal? chikouSpan, decimal? senkouSpanA, decimal? senkouSpanB)
        {
            Date = date;
            TenkanSen = tenkanSen;
            KijunSen = kijunSen;
            ChikouSpan = chikouSpan;
            SenkouSpanA = senkouSpanA;
            SenkouSpanB = senkouSpanB;
        }
    }
}
