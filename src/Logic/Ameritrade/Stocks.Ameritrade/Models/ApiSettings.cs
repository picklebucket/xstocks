using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Ameritrade.Models
{
    public class ApiSettings
    {
        public string RefreshToken { get; set; }
        public string ClientId { get; set; }
        public string BaseUrl { get; set; }
    }
}
