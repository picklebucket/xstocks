using Stocks.Pipelines.Models;
using Stocks.Pipelines.Providers;
using Stocks.Pipelines.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stocks.Pipelines.Services
{
    public class PipelineManagerService : IPipelineManagerService
    {
        private readonly IPipelineInfoProvider _pipelineInfoProvider;
        private readonly IServiceProvider _serviceProvider;
        private readonly Dictionary<string, PipelineInfo> _pipelines;

        public PipelineManagerService(IServiceProvider serviceProvider, PipelineSettings pipelineSettings)
        {
            _pipelineInfoProvider = PipelineInfoProviderFactory.GetProvider(pipelineSettings);
            _pipelines = new Dictionary<string, PipelineInfo>();
            _serviceProvider = serviceProvider;
        }

        public async Task RunAsync(string pipelineName, PipelineArgs args)
        {
            var pipeline = GetPipeline(pipelineName);
            var pipelineMethods = new List<PipelineMethodInfo>();
            foreach (ProcessorInfo processor in pipeline.Processors)
            {
                var processorInstance = processor.Resolve ? _serviceProvider.GetService(processor.ProcessorType) : Activator.CreateInstance(processor.ProcessorType);
                var methodInfo = processor.ProcessorType.GetMethod(processor.MethodName);
                if (methodInfo == null)
                {
                    throw new Exception($"Type {processor.ProcessorType} doesn't have {processor.MethodName} method");
                }
                pipelineMethods.Add(new PipelineMethodInfo(processorInstance, methodInfo, processor.Async));
            }

            foreach (var pipelineMethod in pipelineMethods)
            {
                if(pipelineMethod.Async)
                {
                    await pipelineMethod.InvokeAsync(args);
                }
                else
                {
                    pipelineMethod.Invoke(args);
                }
                
                if (args.Aborted)
                {
                    break;
                }
            }
        }

        private PipelineInfo GetPipeline(string pipelineName)
        {
            PipelineInfo pipeline;;
            if (_pipelines.ContainsKey(pipelineName))
            {
                pipeline = _pipelines[pipelineName];
            }
            else
            {
                var processors = _pipelineInfoProvider.GetProcessors(pipelineName);
                pipeline = new PipelineInfo(pipelineName, processors);
                _pipelines.Add(pipelineName, pipeline);
            }

            return pipeline;
        }
    }
}
