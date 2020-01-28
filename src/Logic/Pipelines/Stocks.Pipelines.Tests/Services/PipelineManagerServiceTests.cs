using Stocks.Pipelines.Services.Interfaces;
using Stocks.Pipelines.Tests.Fixtures;
using Stocks.Pipelines.Tests.Fixtures.Pipelines;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using Xunit.Abstractions;

namespace Stocks.Pipelines.Tests.Services
{
    public class PipelineManagerServiceTests : IClassFixture<PipelinesFixture>
    {
        private readonly IPipelineManagerService _pipelineManagerService;
        private readonly ITestOutputHelper _testOutputHelper;

        public PipelineManagerServiceTests(PipelinesFixture pipelinesFixture, ITestOutputHelper testOutputHelper)
        {
            _pipelineManagerService = pipelinesFixture.PipelineManagerService;
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void RunCustomPipeline()
        {
            var args = new CustomPipelineArgs();
            _pipelineManagerService.Run("custom", args);
            Assert.Equal(Messages.CustomMessage, args.ResponseMessage);
            _testOutputHelper.WriteLine(args.ResponseMessage);
        }

        [Fact]
        public void RunAbortPipeline()
        {
            var args = new CustomPipelineArgs();
            _pipelineManagerService.Run("abort", args);
            Assert.Equal(Messages.AbortMessage, args.ResponseMessage);
            _testOutputHelper.WriteLine(args.ResponseMessage);
        }
    }
}
