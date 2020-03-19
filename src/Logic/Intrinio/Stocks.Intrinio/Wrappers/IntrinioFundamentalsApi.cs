using Intrinio.SDK.Api;
using Intrinio.SDK.Client;
using Stocks.Intrinio.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Intrinio.Wrappers
{
    public class IntrinioFundamentalsApi : FundamentalsApi
    {
        private IntrinioFundamentalsApi(Configuration configuration) : base(configuration)
        {

        }

        public static IntrinioFundamentalsApi Create(IntrinioApiSettings settings)
        {
            return new IntrinioFundamentalsApi(IntrinioConfiguration.Create(settings));
        }
    }
}
