using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Pipelines.Tests.Fixtures.Pipelines
{
    class AsyncProcessor : AsyncProcessor<CustomPipelineArgs>
    {
        public override async Task ExecuteAsync(CustomPipelineArgs args)
        {
            args.ResponseMessage = Messages.AsyncMessage;
            await Task.CompletedTask;
        }
    }
}
