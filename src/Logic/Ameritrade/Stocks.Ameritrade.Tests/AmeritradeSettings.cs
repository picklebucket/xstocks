using Microsoft.Extensions.Configuration;
using Stocks.Ameritrade.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Ameritrade.Tests
{
    public static class AmeritradeSettings
    {
        public static AmeritradeApiSettings GetSettings()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.json.user", true)
                .Build();
            var ameritradeSection = configuration.GetSection("Ameritrade");
            var apiSettings = new AmeritradeApiSettings();
            ameritradeSection.Bind(apiSettings);
            return apiSettings;
        }
    }
}
