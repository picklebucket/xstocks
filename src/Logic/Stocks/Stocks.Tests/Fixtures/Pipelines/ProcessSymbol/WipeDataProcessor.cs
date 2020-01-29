using Stocks.Data.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Tests.Fixtures.Pipelines.ProcessSymbol
{
    class WipeDataProcessor
    {
        private readonly IPriceHistoryRepository _priceHistoryRepository;

        public WipeDataProcessor(IPriceHistoryRepository priceHistoryRepository)
        {
            _priceHistoryRepository = priceHistoryRepository;
        }

        public void Execute(ProcessSymbolPipelineArgs args)
        {
            _priceHistoryRepository.DeleteAsync(args.Symbol);
        }
    }
}
