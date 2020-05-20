using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.NewsApi.Models
{
    public class NewsDetailsModel
    {
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime? PublishedAt { get; set; }
    }
}
