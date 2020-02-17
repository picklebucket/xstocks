using System.Threading.Tasks;

namespace Stocks.Pipelines.Tests.Fixtures.Pipelines
{
    public class AsyncProcessor : AsyncProcessor<CustomPipelineArgs>
    {
        public override Task ExecuteAsync(CustomPipelineArgs args)
        {
            args.ResponseMessage = Messages.AsyncMessage;
            return Task.CompletedTask;
        }
    }
}
