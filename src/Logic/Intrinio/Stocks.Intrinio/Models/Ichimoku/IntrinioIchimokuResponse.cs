using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Intrinio.Models.Ichimoku
{
    public class IchimokuTechnical
    {
        public DateTimeOffset? Date { get; set; }
        public decimal? TenkanSen { get; set; }
        public decimal? KijunSen { get; set; }
        /// <summary>
        /// Lagging line
        /// </summary>
        public decimal? ChikouSpan { get; set; }
        public decimal? SenkouSpanA { get; set; }
        public decimal? SenkouSpanB { get; set; }
    }

    public class IntrinioIchimokuResponse
    {
        public IEnumerable<IchimokuTechnical> Technicals { get; set; }
        public string NextPage { get; set; }
    }
}
