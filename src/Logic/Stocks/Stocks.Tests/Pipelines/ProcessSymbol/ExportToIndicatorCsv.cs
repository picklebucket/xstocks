using Stocks.Pipelines;
using Stocks.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Stocks.Tests.Pipelines.ProcessSymbol
{
    public class ExportToIndicatorCsv : Processor<ProcessSymbolPipelineArgs>
    {
        private readonly IReportService _reportService;

        public ExportToIndicatorCsv(IReportService reportService)
        {
            _reportService = reportService;
        }

        public override void Execute(ProcessSymbolPipelineArgs args)
        {
            byte[] csvContent = _reportService.ExportToIndicatorCsv(args.ReportItems);
            string fileName = $"{args.Symbol}.indicators.csv";
            string filePath = Path.GetFullPath(fileName);
            File.WriteAllText(filePath, Encoding.Default.GetString(csvContent));
            args.Messages.Add($"Indicators were saved to: {filePath}");
        }
    }
}
