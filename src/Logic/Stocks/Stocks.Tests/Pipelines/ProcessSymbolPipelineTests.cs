using Stocks.Ameritrade.Models.PriceHistory;
using Stocks.Data.Entities;
using Stocks.Data.Repositories.Interfaces;
using Stocks.Pipelines.Services.Interfaces;
using Stocks.Tests.Fixtures;
using Stocks.Tests.Fixtures.Pipelines.ProcessSymbol;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Stocks.Tests.Pipelines
{
    public class ProcessSymbolPipelineTests : IClassFixture<StocksFixture>
    {
        private readonly IPipelineManagerService _pipelineManagerService;
        private readonly IPriceHistoryRepository _priceHistoryRepository;

        public ProcessSymbolPipelineTests(StocksFixture stocksFixture)
        {
            _pipelineManagerService = stocksFixture.PipelineManagerService;
            _priceHistoryRepository = stocksFixture.PriceHistoryRepository;
        }

        [Fact]
        public void Run()
        {
            var args = new ProcessSymbolPipelineArgs()
            {
                Symbol = "NFLX",
                PriceHistoryRequest = new PriceHistoryRequest()
                {
                    PeriodType = PeriodType.Year,
                    FrequencyType = FrequencyType.Daily,
                    Period = 1
                }
            };
            _pipelineManagerService.RunAsync("processSymbol", args);

            PriceHistory priceHistory = _priceHistoryRepository.GetAsync(args.Symbol).Result;
            Assert.NotNull(priceHistory);
            Assert.NotEmpty(priceHistory.Candles);
        }
    }
}
