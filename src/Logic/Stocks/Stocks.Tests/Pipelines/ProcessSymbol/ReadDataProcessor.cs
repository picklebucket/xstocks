using Stocks.Pipelines;
using Stocks.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Tests.Pipelines.ProcessSymbol
{
    public class ReadDataProcessor : AsyncProcessor<ProcessSymbolPipelineArgs>
    {
        private readonly IPriceHistoryDataService _priceHistoryService;

        public ReadDataProcessor(IPriceHistoryDataService priceHistoryService)
        {
            _priceHistoryService = priceHistoryService;
        }

        public override async Task ExecuteAsync(ProcessSymbolPipelineArgs args)
        {
            args.CandleItems.AddRange(await _priceHistoryService.GetCandlesAsync(args.PriceHistoryRequest.Symbol).ConfigureAwait(false));
        }
    }
}
