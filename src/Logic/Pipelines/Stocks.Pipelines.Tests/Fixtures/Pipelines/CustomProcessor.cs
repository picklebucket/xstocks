using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Pipelines.Tests.Fixtures.Pipelines
{
    public class CustomProcessor : Processor<CustomPipelineArgs>
    {
        public override void Execute(CustomPipelineArgs pipelineArgs)
        {
            pipelineArgs.ResponseMessage = Messages.CustomMessage;
        }
    }
}
