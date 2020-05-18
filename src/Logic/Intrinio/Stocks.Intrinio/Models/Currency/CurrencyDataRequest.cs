using System;

namespace Stocks.Intrinio.Models.Currency
{
    public enum CurrencyPairTimeframeType
    {
        Default,
        Min1,
        Min5,
        Min15,
        Min30,
        H1,
        H2,
        H3,
        H4,
        H6,
        H8,
        D1,
        W1,
        Month1
    }

    public static class CurrencyPairTimeframe
    {
        public static readonly string Min1 = "m1";
        public static readonly string Min5 = "m5";
        public static readonly string Min15 = "m15";
        public static readonly string Min30 = "m30";
        public static readonly string H1 = "H1";
        public static readonly string H2 = "H2";
        public static readonly string H3 = "H3";
        public static readonly string H4 = "H4";
        public static readonly string H6 = "H6";
        public static readonly string H8 = "H8";
        public static readonly string D1 = "D1";
        public static readonly string W1 = "W1";
        public static readonly string Month1 = "M1";
    }

    public class CurrencyDataRequest
    {
        public string PairCode { get; set; }

        public CurrencyPairTimeframeType Timeframe { get; set; }

        public string Timezone { get; set; } = "UTC";

        public DateTime? StartDate { get; set; }

        public string StartTime { get; set; }

        public DateTime? EndDate { get; set; }

        public string EndTime { get; set; }

        public int? PageSize { get; set; }
    }
}
