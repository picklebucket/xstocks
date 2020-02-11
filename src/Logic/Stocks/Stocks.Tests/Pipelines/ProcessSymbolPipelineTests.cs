using Stocks.Ameritrade.Models.PriceHistory;
using Stocks.Data.Entities;
using Stocks.Data.Repositories.Interfaces;
using Stocks.Pipelines.Services.Interfaces;
using Stocks.Tests.Fixtures;
using Stocks.Tests.Fixtures.Pipelines.ProcessSymbol;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using System.Text.Json;

namespace Stocks.Tests.Pipelines
{
    [Collection(Constants.CollectionName)]
    public class ProcessSymbolPipelineTests : BaseTests
    {
        private readonly IPipelineManagerService _pipelineManagerService;
        private readonly IPriceHistoryRepository _priceHistoryRepository;
        private readonly ITestOutputHelper _testOutputHelper;

        public ProcessSymbolPipelineTests(StocksFixture stocksFixture, ITestOutputHelper testOutputHelper) : base(stocksFixture)
        {
            _pipelineManagerService = ServiceProvider.GetService<IPipelineManagerService>();
            _priceHistoryRepository = ServiceProvider.GetService<IPriceHistoryRepository>();
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task Run()
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
            await _pipelineManagerService.RunAsync("processSymbol", args);

            PriceHistory priceHistory = await _priceHistoryRepository.GetAsync(args.Symbol);
            Assert.NotNull(priceHistory);
            Assert.NotEmpty(priceHistory.Candles);

            _testOutputHelper.WriteLine(JsonSerializer.Serialize(args.ReportItems, new JsonSerializerOptions() { WriteIndented = true }));
        }
    }
}
