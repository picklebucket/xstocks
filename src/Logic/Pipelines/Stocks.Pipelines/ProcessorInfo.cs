using System;

namespace Stocks.Pipelines
{
    public class ProcessorInfo
    {
        public Type ProcessorType { get; private set; }
        public bool Resolve { get; private set; }
        public string MethodName { get; private set; }

        public ProcessorInfo(Type processorType, bool resolve = false, string methodName = "Execute")
        {
            ProcessorType = processorType;
            Resolve = resolve;
            MethodName = methodName;
        }
    }
}
