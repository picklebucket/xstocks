using System;
using Microsoft.Extensions.Configuration;
using Stocks.Interinio.Models;
using Stocks.Interinio.Services.Interfaces;

namespace Stocks.Interinio.Services
{
    public class SettingsService : ISettingsService
    {
        public SettingsService()
        {

        }
        public SettingsModel GetSettings()
        {
            var configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.json.user", true)
                .Build();
            var intrinioSection = configuration.GetSection("Intrinio");
            var apiSettings = new SettingsModel();
            intrinioSection.Bind(apiSettings);
            return apiSettings;
        }
    }
}
