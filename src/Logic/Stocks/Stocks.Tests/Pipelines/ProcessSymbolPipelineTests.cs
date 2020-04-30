using Stocks.Ameritrade.Models.PriceHistory;
using Stocks.Data.Entities;
using Stocks.Data.Repositories.Interfaces;
using Stocks.Pipelines.Services.Interfaces;
using Stocks.Tests.Fixtures;
using System.Threading.Tasks;
using Xunit;
using Microsoft.Extensions.DependencyInjection;
using Xunit.Abstractions;
using System.Text.Json;
using Stocks.Tests.Pipelines.ProcessSymbol;
using Stocks.Models.Api.PriceHistory;
using Stocks.Models.Api;
using System;

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
        public async Task RunProcessSymbolPipeline()
        {
            var args = new ProcessSymbolPipelineArgs()
            {
                PriceHistoryRequest = new PriceHistoryRequest("AAPL", new DateTime(2018, 1, 1), new DateTime(2019, 1, 1), FrequencyType.Daily)
        };
            await _pipelineManagerService.RunAsync("processSymbol", args).ConfigureAwait(false);

            PriceHistory priceHistory = await _priceHistoryRepository.GetAsync(args.PriceHistoryRequest.Symbol).ConfigureAwait(false);
            Assert.NotNull(priceHistory);
            Assert.NotEmpty(priceHistory.Candles);

            foreach (var message in args.Messages)
            {
                _testOutputHelper.WriteLine(message);
            }
            _testOutputHelper.WriteLine("Report: ");
            _testOutputHelper.WriteLine(JsonSerializer.Serialize(args.ReportItems, new JsonSerializerOptions() { WriteIndented = true }));
        }
    }
}
