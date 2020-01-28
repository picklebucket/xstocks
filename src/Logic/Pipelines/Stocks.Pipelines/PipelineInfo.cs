using System;
using System.Collections.Generic;

namespace Stocks.Pipelines
{
    public class PipelineInfo
    {
        private readonly List<ProcessorInfo> _processors;

        public PipelineInfo(IEnumerable<ProcessorInfo> processors)
        {
            _processors = new List<ProcessorInfo>(processors);
        }

        public void Run(PipelineArgs pipelineArgs)
        {
            var pipelineMethods = new List<PipelineMethodInfo>();
            foreach (var processor in _processors)
            {
                var processorInstance = Activator.CreateInstance(processor.ProcessorType);
                var methodInfo = processor.ProcessorType.GetMethod(processor.MethodName);
                if(methodInfo == null)
                {
                    throw new Exception($"Type {processor.ProcessorType} doesn't have {processor.MethodName} method");
                }
                pipelineMethods.Add(new PipelineMethodInfo(processorInstance, methodInfo));
            }

            foreach (var pipelineMethod in pipelineMethods)
            {
                pipelineMethod.Invoke(new[] { pipelineArgs});
                if(pipelineArgs.Aborted)
                {
                    break;
                }
            }
        }
    }
}
