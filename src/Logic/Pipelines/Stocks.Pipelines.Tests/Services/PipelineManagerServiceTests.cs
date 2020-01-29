using Stocks.Pipelines.Models;
using Stocks.Pipelines.Services.Interfaces;
using Stocks.Pipelines.Tests.Fixtures;
using Stocks.Pipelines.Tests.Fixtures.Pipelines;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Stocks.Pipelines.Tests.Services
{
    public class PipelineManagerServiceTests : IClassFixture<PipelinesFixture>
    {
        private readonly IPipelineManagerService _pipelineManagerService;
        private readonly PipelineSettings _pipelineSettings;
        private readonly ITestOutputHelper _testOutputHelper;

        public PipelineManagerServiceTests(PipelinesFixture pipelinesFixture, ITestOutputHelper testOutputHelper)
        {
            _pipelineManagerService = pipelinesFixture.PipelineManagerService;
            _pipelineSettings = pipelinesFixture.PipelineSettings;
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task RunCustomPipeline()
        {
            var args = new CustomPipelineArgs();
            await _pipelineManagerService.RunAsync("custom", args);
            Assert.Equal(Messages.CustomMessage, args.ResponseMessage);
            Assert.False(args.Aborted);
            _testOutputHelper.WriteLine(args.ResponseMessage);
        }

        [Fact]
        public async Task RunAbortPipeline()
        {
            var args = new CustomPipelineArgs();
            await _pipelineManagerService.RunAsync("abort", args);
            Assert.Equal(Messages.AbortMessage, args.ResponseMessage);
            Assert.True(args.Aborted);
            _testOutputHelper.WriteLine(args.ResponseMessage);
        }

        [Fact]
        public async Task RunDIPipeline()
        {
            var args = new CustomPipelineArgs();
            await _pipelineManagerService.RunAsync("di", args);
            Assert.Equal(_pipelineSettings.ConfigFilePath, args.ResponseMessage);
            Assert.False(args.Aborted);
        }

        [Fact]
        public async Task RunAsyncPipeline()
        {
            var args = new CustomPipelineArgs();
            await _pipelineManagerService.RunAsync("async", args);
            Assert.Equal(Messages.AsyncMessage, args.ResponseMessage);
            Assert.False(args.Aborted);
        }
    }
}
