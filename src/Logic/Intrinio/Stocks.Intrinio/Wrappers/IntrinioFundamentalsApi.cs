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
        public IntrinioFundamentalsApi(IntrinioApiSettings settings) : base(IntrinioConfiguration.Create(settings))
        {

        }
    }
}
