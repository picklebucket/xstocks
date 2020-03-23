using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Models.Api.Ichimoku
{
    public class IchimokuRequest
    {
        public string Symbol { get; }
        public DateTime? StartDate { get; }
        public DateTime? EndDate { get; }
        public FrequencyType Frequency { get; }

        public IchimokuRequest(string symbol, DateTime? startDate, DateTime? endDate, FrequencyType frequency)
        {
            Symbol = symbol;
            StartDate = startDate;
            EndDate = endDate;
            Frequency = frequency;
        }
    }
}
