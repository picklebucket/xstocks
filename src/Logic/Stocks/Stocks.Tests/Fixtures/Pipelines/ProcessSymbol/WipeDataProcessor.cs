using Stocks.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Tests.Fixtures.Pipelines.ProcessSymbol
{
    class WipeDataProcessor
    {
        private readonly IPriceHistoryRepository _priceHistoryRepository;

        public WipeDataProcessor(IPriceHistoryRepository priceHistoryRepository)
        {
            _priceHistoryRepository = priceHistoryRepository;
        }

        public async Task ExecuteAsync(ProcessSymbolPipelineArgs args)
        {
            await _priceHistoryRepository.DeleteAsync(args.Symbol);
        }
    }
}
