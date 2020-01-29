using Stocks.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Tests.Fixtures.Pipelines.ProcessSymbol
{
    class CollectDataProcessor
    {
        private readonly ICollectingService _collectingService;

        public CollectDataProcessor(ICollectingService collectingService)
        {
            _collectingService = collectingService;
        }

        public void Execute(ProcessSymbolPipelineArgs args)
        {
            _collectingService.CollectDataAsync(args.Symbol, args.PriceHistoryRequest).Wait();
        }
    }
}
