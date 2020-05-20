using System;

namespace Stocks.Data.Entities
{
    public enum SentimentAnalysisType
    {
        Default,
        Negative,
        Netural,
        Positive
    }

    public class NewsDetails : BaseEntity
    {
        public string Title { get; set; }
     
        public string Text { get; set; }

        public SentimentAnalysisType SentimentAnalysis { get; set; }

        public DateTime? PublishedAt { get; set; }
    } 
}
