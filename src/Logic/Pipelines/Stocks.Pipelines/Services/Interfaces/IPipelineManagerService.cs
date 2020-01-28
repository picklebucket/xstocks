using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Pipelines.Services.Interfaces
{
    public interface IPipelineManagerService
    {
        void Run(string pipelineName, PipelineArgs args);
    }
}
