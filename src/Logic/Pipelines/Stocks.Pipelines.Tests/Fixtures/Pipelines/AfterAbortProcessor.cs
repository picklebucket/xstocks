using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Pipelines.Tests.Fixtures.Pipelines
{
    class AfterAbortProcessor
    {
        public void Execute(CustomPipelineArgs pipelineArgs)
        {
            pipelineArgs.ResponseMessage = Messages.AfterAbortMessage;
        }
    }
}
