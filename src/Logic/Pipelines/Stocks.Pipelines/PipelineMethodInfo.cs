using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Pipelines
{
    public class PipelineMethodInfo
    {
        public bool Async { get; }

        private readonly object _instance;
        private readonly MethodInfo _methodInfo;
        
        public PipelineMethodInfo(object instance, MethodInfo methodInfo, bool async)
        {
            _instance = instance;
            _methodInfo = methodInfo;
            Async = async;
        }

        public object Invoke(params object[] parameters)
        {
            return _methodInfo.Invoke(_instance, parameters);
        }

        public async Task InvokeAsync(params object[] parameters)
        {
            var task = (Task)Invoke(parameters);
            await task;
        }
    }
}
