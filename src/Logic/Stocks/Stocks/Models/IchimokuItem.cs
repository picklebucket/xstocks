using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Models
{
    public class IchimokuItem
    {
        public DateTimeOffset Date { get; set; }
        public double? TenkanSen { get; set; }
        public double? KijunSen { get; set; }
        /// <summary>
        /// Lagging line
        /// </summary>
        public double? ChikouSpan { get; set; }
        public double? SenkouSpanA { get; set; }
        public double? SenkouSpanB { get; set; }

        public IchimokuItem()
        {
        }

        public IchimokuItem(DateTimeOffset date, double? tenkanSen, double? kijunSen, double? chikouSpan, double? senkouSpanA, double? senkouSpanB)
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
