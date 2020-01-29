using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stocks.Pipelines.DI;
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
        public PipelineSettings PipelineSettings { get; }

        public PipelinesFixture()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.json.user", true)
                .Build();
            var pipelinesSection = configuration.GetSection("Pipelines");
            var pipelineSettings = new PipelineSettings();
            pipelinesSection.Bind(pipelineSettings);

            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<IPipelineManagerService, PipelineManagerService>();
            serviceCollection.AddSingleton(pipelineSettings);
            serviceCollection.AddPipelineProcessors(pipelineSettings);

            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();

            PipelineSettings = pipelineSettings;
            PipelineManagerService = serviceProvider.GetService<IPipelineManagerService>();
        }
    }
}
