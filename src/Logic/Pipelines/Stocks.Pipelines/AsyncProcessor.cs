using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Pipelines
{
    public abstract class AsyncProcessor<TPipelineArgs>
        where TPipelineArgs : PipelineArgs
    {
        public abstract Task ExecuteAsync(TPipelineArgs args);
    }
}
