using System;
using System.Collections.Generic;

namespace Stocks.Pipelines
{
    public class PipelineInfo
    {
        public List<ProcessorInfo> Processors { get; }
        public string Name { get; private set; }

        public PipelineInfo(string name, IEnumerable<ProcessorInfo> processors)
        {
            Processors = new List<ProcessorInfo>(processors);
            Name = name;
        }        
    }
}
