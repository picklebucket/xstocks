using Stocks.Pipelines;
using Stocks.Services.Collecting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Tests.Pipelines.ProcessSymbol
{
    public class CollectDataProcessor : AsyncProcessor<ProcessSymbolPipelineArgs>
    {
        private readonly ICollectingService _collectingService;

        public CollectDataProcessor(ICollectingService collectingService)
        {
            _collectingService = collectingService;
        }

        public override async Task ExecuteAsync(ProcessSymbolPipelineArgs args)
        {
            await _collectingService.CollectDataAsync(args.PriceHistoryRequest).ConfigureAwait(false);
        }
    }
}
