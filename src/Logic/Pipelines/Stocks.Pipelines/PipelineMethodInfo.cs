using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace Stocks.Pipelines
{
    public class PipelineMethodInfo
    {
        private readonly object _instance;
        private readonly MethodInfo _methodInfo;
        public PipelineMethodInfo(object instance, MethodInfo methodInfo)
        {
            _instance = instance;
            _methodInfo = methodInfo;
        }

        public void Invoke(object[] parameters)
        {
            //TODO::add posibility for async pipeline methods
            _methodInfo.Invoke(_instance, parameters);
        }
    }
}
