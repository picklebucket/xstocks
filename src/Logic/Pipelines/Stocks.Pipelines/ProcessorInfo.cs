using System;

namespace Stocks.Pipelines
{
    public class ProcessorInfo
    {
        public Type ProcessorType { get; private set; }
        public string MethodName { get; private set; }

        public ProcessorInfo(Type processorType, string methodName = "Execute")
        {
            ProcessorType = processorType;
            MethodName = methodName;
        }
    }
}
