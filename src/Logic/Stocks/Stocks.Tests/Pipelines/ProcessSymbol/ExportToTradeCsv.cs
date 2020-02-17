using Stocks.Pipelines;
using Stocks.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Stocks.Tests.Pipelines.ProcessSymbol
{
    public class ExportToTradeCsv : Processor<ProcessSymbolPipelineArgs>
    {
        private readonly IReportService _reportService;

        public ExportToTradeCsv(IReportService reportService)
        {
            _reportService = reportService;
        }

        public override void Execute(ProcessSymbolPipelineArgs args)
        {
            var csvContent = _reportService.ExportToTradeCsv(args.ReportItems);
            string fileName = $"{args.Symbol}.trades.csv";
            string filePath = Path.GetFullPath(fileName);
            File.WriteAllText(filePath, Encoding.Default.GetString(csvContent));
            args.Messages.Add($"Trades were saved to: {filePath}");
        }
    }
}
