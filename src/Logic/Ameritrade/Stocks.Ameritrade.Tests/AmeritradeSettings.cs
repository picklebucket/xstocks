using Microsoft.Extensions.Configuration;
using Stocks.Ameritrade.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Ameritrade.Tests
{
    public static class AmeritradeSettings
    {
        public static ApiSettings GetSettings()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.json.user", true)
                .Build();
            var ameritradeSection = configuration.GetSection("Ameritrade");
            var apiSettings = new ApiSettings();
            ameritradeSection.Bind(apiSettings);
            return apiSettings;
        }
    }
}
