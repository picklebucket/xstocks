using System;
using System.IO;
using System.Text;
using Intrinio.SDK.Api;
using Intrinio.SDK.Client;
using Intrinio.SDK.Model;
using Stocks.Interinio.Models;
using Stocks.Interinio.Services.Interfaces;

namespace Stocks.Interinio.Services
{
    public abstract class IntrinioApiBase
    {
        //private Configuration _connection;


        public IntrinioApiBase(SettingsModel settings)
        {
            //TODO add more error handling around this if settings items are missing/null

            if (settings.IsTest == true)
            {
                Configuration.Default.AddApiKey("api_key", settings.SandboxToken);
            }
            else if (settings.IsTest == false)
            {
                Configuration.Default.AddApiKey("api_key", settings.ProdToken);
            }
            

        }
        
    }
}
