using Microsoft.Extensions.DependencyInjection;
using Stocks.Pipelines.Models;
using Stocks.Pipelines.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stocks.Pipelines.DI
{
    public static class ServiceCollectionExtensions
    {
        public static void AddPipelineProcessors(this IServiceCollection serviceCollection, PipelineSettings pipelineSettings)
        {
            if(pipelineSettings == null)
            {
                throw new ArgumentNullException(nameof(pipelineSettings));
            }
            IPipelineInfoProvider pipelineInfoProvider = PipelineInfoProviderFactory.GetProvider(pipelineSettings);
            IEnumerable<ProcessorInfo> diProcessors = pipelineInfoProvider.GetAllProcessors().Where(x => x.Resolve);
            foreach (ProcessorInfo processor in diProcessors)
            {
                serviceCollection.AddTransient(processor.ProcessorType);
            }
        }
    }
}
