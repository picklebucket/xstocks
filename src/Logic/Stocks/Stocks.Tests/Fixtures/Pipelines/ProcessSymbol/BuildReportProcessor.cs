using Stocks.Pipelines;
using Stocks.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Tests.Fixtures.Pipelines.ProcessSymbol
{
    class BuildReportProcessor : Processor<ProcessSymbolPipelineArgs>
    {
        private readonly IReportService _reportService;

        public BuildReportProcessor(IReportService reportService)
        {
            _reportService = reportService;
        }

        public override void Execute(ProcessSymbolPipelineArgs args)
        {
            args.ReportItems = _reportService.BuildReport(args.Symbol, args.CandleItems);
        }
    }
}
