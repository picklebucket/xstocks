using Stocks.Data.Repositories.Interfaces;
using Stocks.Pipelines;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Tests.Fixtures.Pipelines.ProcessSymbol
{
    class WipeDataProcessor : AsyncProcessor<ProcessSymbolPipelineArgs>
    {
        private readonly IPriceHistoryRepository _priceHistoryRepository;

        public WipeDataProcessor(IPriceHistoryRepository priceHistoryRepository)
        {
            _priceHistoryRepository = priceHistoryRepository;
        }

        public override async Task ExecuteAsync(ProcessSymbolPipelineArgs args)
        {
            await _priceHistoryRepository.DeleteAsync(args.Symbol);
        }
    }
}
