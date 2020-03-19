using System;
namespace Stocks.Intrinio.Models
{
    public class IntrinioApiSettings
    {
        public string SandboxToken { get; set; }
        public string ProdToken { get; set; }
        public bool IsTest { get; set; }
        public IntrinioApiSettings()
        {
        }
    }
}
