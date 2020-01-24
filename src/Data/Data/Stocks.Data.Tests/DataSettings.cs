using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Data.Tests
{
    public class DataSettings
    {
        public string Stocks { get; set; }

        public static DataSettings GetSettings()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.json.user", true)
                .Build();
            var connectionStringsSection = configuration.GetSection("ConnectionStrings");
            var dataSettings = new DataSettings();
            connectionStringsSection.Bind(dataSettings);
            return dataSettings;
        }
    }
}
