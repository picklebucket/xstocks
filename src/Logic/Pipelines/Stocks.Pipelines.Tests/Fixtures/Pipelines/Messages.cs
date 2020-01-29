using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Pipelines.Tests.Fixtures.Pipelines
{
    public static class Messages
    {
        public const string CustomMessage = "I am custom processor";
        public const string AbortMessage = "I will abort pipeline";
        public const string AfterAbortMessage = "I shouldn't be called";
        public const string AsyncMessage = "Hello from async processor";
    }
}
