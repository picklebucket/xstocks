using Stocks.Pipelines;
using Stocks.Services.Collecting;
using Stocks.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Tests.Pipelines.ProcessSymbol
{
    public class CollectDataProcessor : AsyncProcessor<ProcessSymbolPipelineArgs>
    {
        private readonly IPriceHistoryService _priceHistoryService;
        private readonly IPriceHistoryDataService _priceHistoryDataService;

        public CollectDataProcessor(IPriceHistoryService priceHistoryService, IPriceHistoryDataService priceHistoryDataService)
        {
            _priceHistoryService = priceHistoryService;
            _priceHistoryDataService = priceHistoryDataService;
        }

        public override async Task ExecuteAsync(ProcessSymbolPipelineArgs args)
        {
            var response = await _priceHistoryService.GetPriceHistoryAsync(args.PriceHistoryRequest).ConfigureAwait(false);
            if(response.IsSuccess)
            {
                await _priceHistoryDataService.CreateAsync(response.Content.Symbol, response.Content.IsEmpty, response.Content.CandleItems).ConfigureAwait(false);
            }
            else
            {
                args.Aborted = true;
                args.Messages.Add($"Can't get price history from api. response message: {response.ErrorMessage}");
            }
        }
    }
}
