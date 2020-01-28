using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Pipelines.Tests.Fixtures.Pipelines
{
    class AbortProcessor
    {
        public void Execute(CustomPipelineArgs pipelineArgs)
        {
            pipelineArgs.ResponseMessage = Messages.AbortMessage;
            pipelineArgs.Aborted = true;
        }
    }
}
