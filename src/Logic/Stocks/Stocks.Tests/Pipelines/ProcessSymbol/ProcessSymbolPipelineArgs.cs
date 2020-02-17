using Stocks.Ameritrade.Models.PriceHistory;
using Stocks.Models;
using Stocks.Pipelines;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Tests.Pipelines.ProcessSymbol
{
    public class ProcessSymbolPipelineArgs : PipelineArgs
    {
        public string Symbol { get; set; }
        public PriceHistoryRequest PriceHistoryRequest { get; set; }
        public List<CandleItem> CandleItems { get; }
        public IEnumerable<ReportItem> ReportItems { get; set; }
        public List<string> Messages { get; }

        public ProcessSymbolPipelineArgs()
        {
            Messages = new List<string>();
            CandleItems = new List<CandleItem>();
        }
    }
}
