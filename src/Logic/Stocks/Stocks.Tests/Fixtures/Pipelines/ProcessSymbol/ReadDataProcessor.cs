using Stocks.Pipelines;
using Stocks.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Tests.Fixtures.Pipelines.ProcessSymbol
{
    class ReadDataProcessor : AsyncProcessor<ProcessSymbolPipelineArgs>
    {
        private readonly IReportService _reportService;

        public ReadDataProcessor(IReportService reportService)
        {
            _reportService = reportService;
        }

        public override async Task ExecuteAsync(ProcessSymbolPipelineArgs args)
        {
            args.CandleItems = await _reportService.GetCollectedCandlesAsync(args.Symbol);
        }
    }
}
