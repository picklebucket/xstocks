using Intrinio.SDK.Api;
using Intrinio.SDK.Client;
using Stocks.Intrinio.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Intrinio.Wrappers
{
    public class IntrinioSecurityApi : SecurityApi
    {
        private IntrinioSecurityApi(Configuration configuration) : base(configuration)
        {

        }

        public static IntrinioSecurityApi Create(IntrinioApiSettings settings)
        {
            return new IntrinioSecurityApi(IntrinioConfiguration.Create(settings));
        }
    }
}
