using Stocks.Pipelines.Models;
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
        private readonly PipelineSettings _pipelineSettings;
        private readonly ITestOutputHelper _testOutputHelper;

        public PipelineManagerServiceTests(PipelinesFixture pipelinesFixture, ITestOutputHelper testOutputHelper)
        {
            _pipelineManagerService = pipelinesFixture.PipelineManagerService;
            _pipelineSettings = pipelinesFixture.PipelineSettings;
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void RunCustomPipeline()
        {
            var args = new CustomPipelineArgs();
            _pipelineManagerService.Run("custom", args);
            Assert.Equal(Messages.CustomMessage, args.ResponseMessage);
            Assert.False(args.Aborted);
            _testOutputHelper.WriteLine(args.ResponseMessage);
        }

        [Fact]
        public void RunAbortPipeline()
        {
            var args = new CustomPipelineArgs();
            _pipelineManagerService.Run("abort", args);
            Assert.Equal(Messages.AbortMessage, args.ResponseMessage);
            Assert.True(args.Aborted);
            _testOutputHelper.WriteLine(args.ResponseMessage);
        }

        [Fact]
        public void RunDIPipeline()
        {
            var args = new CustomPipelineArgs();
            _pipelineManagerService.Run("di", args);
            Assert.Equal(_pipelineSettings.ConfigFilePath, args.ResponseMessage);
            Assert.False(args.Aborted);
        }
    }
}
