using Microsoft.Extensions.DependencyInjection;
using Stocks.Pipelines.DI;
using Stocks.Pipelines.Models;
using Stocks.Pipelines.Services;
using Stocks.Pipelines.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Pipelines.Tests.DI
{
    public static class DIConfiguration
    {
        public static ServiceProvider GetServiceProvider(PipelineSettings pipelineSettings)
        {
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddTransient<IPipelineManagerService, PipelineManagerService>();
            serviceCollection.AddSingleton(pipelineSettings);
            serviceCollection.AddPipelineProcessors(pipelineSettings);

            ServiceProvider serviceProvider = serviceCollection.BuildServiceProvider();
            return serviceProvider;
        }
    }
}
