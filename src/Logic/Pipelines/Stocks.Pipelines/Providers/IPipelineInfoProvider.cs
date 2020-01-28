using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Pipelines.Providers
{
    interface IPipelineInfoProvider
    {
        IEnumerable<ProcessorInfo> GetProcessors(string pipelineName);
    }
}
