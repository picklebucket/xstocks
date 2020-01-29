using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Pipelines.Services.Interfaces
{
    public interface IPipelineManagerService
    {
        Task RunAsync(string pipelineName, PipelineArgs args);
    }
}
