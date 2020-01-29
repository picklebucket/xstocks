using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stocks.Pipelines.Models;
using Stocks.Pipelines.Services.Interfaces;
using Stocks.Pipelines.Tests.DI;

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

            
            PipelineSettings = pipelineSettings;
            var serviceProvider = DIConfiguration.GetServiceProvider(pipelineSettings);
            PipelineManagerService = serviceProvider.GetService<IPipelineManagerService>();
        }
    }
}
