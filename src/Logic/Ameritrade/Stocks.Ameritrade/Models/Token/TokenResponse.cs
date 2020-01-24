using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Stocks.Ameritrade.Models.Token
{
    class TokenResponse
    {
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }
    }
}
