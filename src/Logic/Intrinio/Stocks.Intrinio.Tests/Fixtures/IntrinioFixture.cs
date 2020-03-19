using Microsoft.Extensions.Configuration;
using Stocks.Intrinio.Models;
using Stocks.Intrinio.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Intrinio.Tests.Fixtures
{
    public class IntrinioFixture
    {
        public IntrinioSecurityApi IntrinioSecurityApi { get; }
        public IntrinioFundamentalsApi IntrinioFundamentalsApi { get; }

        public IntrinioFixture()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.json.user", true)
                .Build();
            var intrinioSection = configuration.GetSection("Intrinio");
            var apiSettings = new IntrinioApiSettings();
            intrinioSection.Bind(apiSettings);

            IntrinioSecurityApi = IntrinioSecurityApi.Create(apiSettings);
            IntrinioFundamentalsApi = IntrinioFundamentalsApi.Create(apiSettings);
        }
    }
}
