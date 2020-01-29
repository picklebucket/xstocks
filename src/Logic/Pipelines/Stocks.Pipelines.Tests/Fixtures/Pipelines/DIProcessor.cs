using Stocks.Pipelines.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Pipelines.Tests.Fixtures.Pipelines
{
    class DIProcessor
    {
        private readonly PipelineSettings _pipelineSettings;

        public DIProcessor(PipelineSettings pipelineSettings)
        {
            _pipelineSettings = pipelineSettings;
        }

        public void Execute(CustomPipelineArgs pipelineArgs)
        {
            pipelineArgs.ResponseMessage = _pipelineSettings.ConfigFilePath;
        }
    }
}
