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

        public ProcessSymbolPipelineTests(StocksFixture stocksFixture)
        {
            _pipelineManagerService = stocksFixture.PipelineManagerService;
        }

        [Fact]
        public void Run()
        {
            var args = new ProcessSymbolPipelineArgs();
            _pipelineManagerService.Run("processSymbol", args);
        }
    }
}
