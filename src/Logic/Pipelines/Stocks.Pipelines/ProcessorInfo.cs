using System;

namespace Stocks.Pipelines
{
    public class ProcessorInfo
    {
        public Type ProcessorType { get; private set; }
        public bool Async { get; private set; }
        public bool Resolve { get; private set; }
        public string MethodName { get; private set; }

        public ProcessorInfo(Type processorType, bool async, bool resolve)
        {
            ProcessorType = processorType;
            Async = async;
            Resolve = resolve;
            MethodName = Async ? "ExecuteAsync" : "Execute";
        }
    }
}
