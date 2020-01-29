using Stocks.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Tests.Fixtures.Pipelines.ProcessSymbol
{
    class CollectDataProcessor
    {
        private readonly ICollectingService _collectingService;

        public CollectDataProcessor(ICollectingService collectingService)
        {
            _collectingService = collectingService;
        }

        public async Task ExecuteAsync(ProcessSymbolPipelineArgs args)
        {
            await _collectingService.CollectDataAsync(args.Symbol, args.PriceHistoryRequest);
        }
    }
}
