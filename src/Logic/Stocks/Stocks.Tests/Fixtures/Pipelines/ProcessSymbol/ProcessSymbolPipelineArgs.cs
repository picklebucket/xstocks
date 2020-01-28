using Stocks.Pipelines;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Tests.Fixtures.Pipelines.ProcessSymbol
{
    public class ProcessSymbolPipelineArgs : PipelineArgs
    {
        public string Symbol { get; set; }
    }
}
