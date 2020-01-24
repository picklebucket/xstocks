using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace Stocks.Ameritrade.Models
{
    class BadRequestResponse
    {
        [JsonPropertyName("error")]
        public string ErrorMessage { get; set; }
    }
}
