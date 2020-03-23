using Stocks.Models;
using Stocks.Models.Api.PriceHistory;
using Stocks.Pipelines;
using System.Collections.Generic;

namespace Stocks.Tests.Pipelines.ProcessSymbol
{
    public class ProcessSymbolPipelineArgs : PipelineArgs
    {
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
