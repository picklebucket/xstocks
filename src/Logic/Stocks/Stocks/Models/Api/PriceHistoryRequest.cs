using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Models.Api
{
    public class PriceHistoryRequest
    {
        public string Symbol { get; }
        public PeriodType PeriodType { get; }
        public short? Period { get; }
        public FrequencyType FrequencyType { get; }

        public DateTime? StartDate { get; }
        public DateTime? EndDate { get; }

        public PriceHistoryRequest(string symbol, PeriodType periodType, short? period, FrequencyType frequencyType)
        {
            Symbol = symbol;
            PeriodType = periodType;
            Period = period;
            FrequencyType = frequencyType;
        }

        public PriceHistoryRequest(string symbol, DateTime? startDate, DateTime? endDate, FrequencyType frequencyType)
        {
            Symbol = symbol;
            StartDate = startDate;
            EndDate = endDate;
            FrequencyType = frequencyType;
        }
    }
}
