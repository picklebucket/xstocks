using Stocks.Pipelines.Models;
using Stocks.Pipelines.Providers;
using Stocks.Pipelines.Services.Interfaces;

namespace Stocks.Pipelines.Services
{
    public class PipelineManagerService : IPipelineManagerService
    {
        private readonly IPipelineInfoProvider _pipelineInfoProvider;

        public PipelineManagerService(PipelineSettings pipelineSettings)
        {
            _pipelineInfoProvider = PipelineInfoProviderFactory.GetProvider(pipelineSettings);
        }

        public void Run(string pipelineName, PipelineArgs args)
        {
            var pipeline = GetPipeline(pipelineName);
            pipeline.Run(args);
        }

        private PipelineInfo GetPipeline(string pipelineName)
        {
            var processors = _pipelineInfoProvider.GetProcessors(pipelineName);
            return new PipelineInfo(processors);
        }
    }
}
