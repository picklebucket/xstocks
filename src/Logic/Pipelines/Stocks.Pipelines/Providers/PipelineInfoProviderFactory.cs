using Stocks.Pipelines.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Pipelines.Providers
{
    static class PipelineInfoProviderFactory
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Globalization", "CA1303:Do not pass literals as localized parameters", Justification = "<Pending>")]
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
