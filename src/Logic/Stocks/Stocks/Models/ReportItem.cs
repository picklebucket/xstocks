using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Models
{
    public class ReportItem
    {
        public string Symbol { get; set; }
        public AtrItem AtrItem { get; set; }
        public CandleItem CandleItem { get; set; }
        public IchimokuItem IchimokuItem { get; set; }
        public SignalType SignalType { get; set; }
        public IndicatorItem IndicatorItem { get; set; }
    }

    public class CandleIchiReport
    {
        public decimal? High { get; set; }
        public decimal? Low { get; set; }
        public decimal? Open { get; set; }
        public decimal? Close { get; set; }
        public DateTime? Date { get; set; }
        public long Ticks { get; set; }
        public SignalType SignalType { get; set; }
    }

    public class IchiReportItem
    {
        public DateTime? Date { get; set; }
        public long Ticks { get; set; }

        /// <summary>
        /// Conversion (turning) line
        /// </summary>
        public decimal? TenkanSen { get; set; }
        /// <summary>
        /// Base line
        /// </summary>
        public decimal? KijunSen { get; set; }
        /// <summary>
        /// Lagging line
        /// </summary>
        public decimal? ChikouSpan { get; set; }
        public decimal? SenkouSpanA { get; set; }
        public decimal? SenkouSpanB { get; set; }
    }

    public class IchiReport
    {
        public string Symbol { get; set; }

        public List<CandleIchiReport> Candles { get; set; }

        public List<IchiReportItem> IchiItems { get; set; }

        public IchiReport()
        {
            this.Candles = new List<CandleIchiReport>();
            this.IchiItems = new List<IchiReportItem>();
        }
    }
}
