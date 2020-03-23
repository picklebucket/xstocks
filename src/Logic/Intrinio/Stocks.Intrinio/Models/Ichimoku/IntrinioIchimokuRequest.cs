using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Stocks.Intrinio.Models.Ichimoku
{
    public class IntrinioIchimokuRequest
    {
        public string Identifier { get; }  // string | A Security identifier (Ticker, FIGI, ISIN, CUSIP, Intrinio ID)
        public int? LowPeriod { get; }  // int? | The number of observations, per period, to calculate Tenkan Sen (Conversion Line) of Ichimoku Kinko Hyo (optional)  (default to 9)
        public int? MediumPeriod { get; } // int? | The number of observations, per period, to calculate Kijun Sen (Base Line), Senkou Span A (Leading Span A), and Chikou Span (Lagging Span) of Ichimoku Kinko Hyo (optional)  (default to 26)
        public int? HighPeriod { get; }  // int? | The number of observations, per period, to calculate Senkou Span B (Leading Span B) of Ichimoku Kinko Hyo (optional)  (default to 52)
        public string StartDate { get; }  // string | Return technical indicator values on or after the date (optional) 
        public string EndDate { get; set; }  // string | Return technical indicator values on or before the date (optional) 
        public int? PageSize { get; }  // int? | The number of results to return (optional)  (default to 100)
        public string NextPage { get; set; }  // string | Gets the next page of data from a previous API call (optional) 

        private IntrinioIchimokuRequest()
        {
            LowPeriod = 9;
            MediumPeriod = 26;
            HighPeriod = 52;
            PageSize = Constants.DefaultPageSize;
        }

        public IntrinioIchimokuRequest(string identifier, DateTime? startDate, DateTime? endDate, string nextPage = null) 
            : this()
        {
            Identifier = identifier;
            StartDate = ToDateString(startDate);
            EndDate = ToDateString(endDate);
            NextPage = nextPage;
        }

        private string ToDateString(DateTime? value)
        {
            if(value.HasValue)
            {
                return value.Value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
            }
            return null;
        }
    }
}
