using Stocks.Pipelines.Services;
using Stocks.Pipelines.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Pipelines.Tests.Fixtures
{
    public class PipelinesFixture
    {
        public IPipelineManagerService PipelineManagerService { get; }

        public PipelinesFixture()
        {
            var settings = PipelinesSettings.GetSettings();
            PipelineManagerService = new PipelineManagerService(settings);
        }
    }
}
