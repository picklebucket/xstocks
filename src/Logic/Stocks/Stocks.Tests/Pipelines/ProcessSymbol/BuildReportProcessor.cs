using Stocks.Models.Api.Ichimoku;
using Stocks.Pipelines;
using Stocks.Services.Ichimoku;
using Stocks.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Tests.Pipelines.ProcessSymbol
{
    public class BuildReportProcessor : AsyncProcessor<ProcessSymbolPipelineArgs>
    {
        private readonly IReportService _reportService;
        private readonly IIchimokuService _ichimokuService;

        public BuildReportProcessor(IReportService reportService, IIchimokuService ichimokuService)
        {
            _reportService = reportService;
            _ichimokuService = ichimokuService;
        }

        public override async Task ExecuteAsync(ProcessSymbolPipelineArgs args)
        {
            var ichimokuRequest = new IchimokuRequest(args.PriceHistoryRequest.Symbol, args.PriceHistoryRequest.StartDate, args.PriceHistoryRequest.EndDate, args.PriceHistoryRequest.FrequencyType);
            var ichimokuResponse = await _ichimokuService.BuildIchimokuCloudAsync(ichimokuRequest).ConfigureAwait(false);
            if(ichimokuResponse.IsSuccess)
            {
                args.ReportItems = _reportService.BuildReport(args.PriceHistoryRequest.Symbol, ichimokuResponse.Content.Items, args.CandleItems);
            }
            else
            {
                args.Aborted = true;
                args.Messages.Add($"Can't build Ichimoku cloud. Message: {ichimokuResponse.ErrorMessage}");
            }
        }
    }
}
