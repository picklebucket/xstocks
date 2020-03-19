using Intrinio.SDK.Client;
using Stocks.Intrinio.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Intrinio.Wrappers
{
    static class IntrinioConfiguration
    {
        public static Configuration Create(IntrinioApiSettings settings)
        {
            var configuration = Configuration.Default;
            configuration.AddApiKey("api_key", settings.IsTest ? settings.SandboxToken : settings.ProdToken);
            return configuration;
        }
    }
}
