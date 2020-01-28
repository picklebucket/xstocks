using Microsoft.Extensions.Configuration;
using Stocks.Pipelines.Models;
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
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.json.user", true)
                .Build();
            var pipelinesSection = configuration.GetSection("Pipelines");
            var pipelineSettings = new PipelineSettings();
            pipelinesSection.Bind(pipelineSettings);
            PipelineManagerService = new PipelineManagerService(pipelineSettings);
        }
    }
}
