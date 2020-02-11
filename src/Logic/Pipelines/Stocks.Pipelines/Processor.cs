using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Pipelines
{
    public abstract class Processor<TPipelineArgs>
        where TPipelineArgs : PipelineArgs
    {
        public abstract void Execute(TPipelineArgs args);
    }
}
