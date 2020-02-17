using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Ameritrade.Models
{
    public class AmeritradeApiSettings
    {
        public string RefreshToken { get; set; }
        public string ClientId { get; set; }
        public Uri BaseUrl { get; set; }
    }
}
