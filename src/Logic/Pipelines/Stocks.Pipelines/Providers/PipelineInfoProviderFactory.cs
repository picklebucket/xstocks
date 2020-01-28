using Stocks.Pipelines.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Pipelines.Providers
{
    static class PipelineInfoProviderFactory
    {
        public static IPipelineInfoProvider GetProvider(PipelineSettings pipelineSettings)
        {
            return pipelineSettings.ConfigFileType switch
            {
                PipelineInfoProviderType.Xml => new XmlPipelineInfoProvider(pipelineSettings.ConfigFilePath),
                _ => throw new NotImplementedException("Unknown provider type"),
            };
        }
    }
}
