using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace Stocks.Stocktwits
{
    public class StocktwitsMessage
    {
        public int Id { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public StocktwitsUser User { get; set; }
        public List<StocktwitsSymbol> Symbols { get; set; }
    }

    public class StocktwitsUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
    }

    public class StocktwitsSymbol
    {
        public int Id { get; set; }
        public string Symbol { get; set; }
        public string Title { get; set; }
    }

    public class StocktwitsResponse
    {
        public List<StocktwitsMessage> Messages { get; set; }
    }

    public class StocktwitsService
    {
        public async Task<StocktwitsResponse> GetTwits(string symbol)
        {
            string url = $"https://api.stocktwits.com/api/2/streams/symbol/{symbol}.json";

            using var client = new HttpClient();

            var response = await client.GetAsync(url);
            var content = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<StocktwitsResponse>(content);
        }
    }
}
