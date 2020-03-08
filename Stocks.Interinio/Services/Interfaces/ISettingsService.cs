using System;
using Stocks.Interinio.Models;

namespace Stocks.Interinio.Services.Interfaces
{
    public interface ISettingsService
    {
        SettingsModel GetSettings();
    }
}
