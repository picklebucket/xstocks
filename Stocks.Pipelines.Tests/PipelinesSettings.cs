using Microsoft.Extensions.Configuration;
using Stocks.Pipelines.Models;

namespace Stocks.Pipelines.Tests
{
    public static class PipelinesSettings
    {
        public static PipelineSettings GetSettings()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.json.user", true)
                .Build();
            var connectionStringsSection = configuration.GetSection("Pipelines");
            var pipelineSettings = new PipelineSettings();
            connectionStringsSection.Bind(pipelineSettings);
            return pipelineSettings;
        }
    }
}
