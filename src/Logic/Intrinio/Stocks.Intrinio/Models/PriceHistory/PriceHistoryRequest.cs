using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Intrinio.Models.PriceHistory
{
    public class PriceHistoryRequest
    {
        public string Identifier { get; }
        public DateTime StartDate { get; }
        public DateTime EndDate { get; }
        public FrequencyType Frequency { get; }
        public int? PageSize { get; }  // int? | The number of results to return (optional)  (default to 100)
        public string NextPage { get; set; }  // string | Gets the next page of data from a previous API call (optional) 

        private PriceHistoryRequest()
        {
            PageSize = Constants.DefaultPageSize;
        }

        public PriceHistoryRequest(string identifier, DateTime startDate, DateTime endDate, FrequencyType frequency) : this()
        {
            Identifier = identifier;
            StartDate = startDate;
            EndDate = endDate;
            Frequency = frequency;
        }
    }
}
