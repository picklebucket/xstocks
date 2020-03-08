using System;
namespace Stocks.Interinio.Models
{
    public class SettingsModel
    {
        public string SandboxToken { get; set; }
        public string ProdToken { get; set; }
        public string BaseUrl { get; set; }
        public bool IsTest { get; set; }
        public SettingsModel()
        {
        }
    }
}
