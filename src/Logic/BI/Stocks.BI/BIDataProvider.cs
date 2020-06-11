using Newtonsoft.Json;
using Stocks.Models;
using Stocks.Models.Api;
using Stocks.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.BI
{
    public class BIDataProvider
    {
        private readonly IReportService _reportService;

        public BIDataProvider(IReportService reportService)
        {
            _reportService = reportService;
        }

        public async Task SendEMA50DataToBI()
        {
            List<SignalReportItem> data = await _reportService.Build50EmaBullReportAsync("MSFT", new DateTime(2018, 1, 1), new DateTime(2020, 5, 25),
                FrequencyType.Daily);

            int index = 0;
            int pages = (int)Math.Ceiling(data.Count / 100.0);
            while (index < pages)
            {
                var batch = data.Skip(100 * index).Take(100).ToList();

                string url = "https://api.powerbi.com/beta/dfefab4d-aaf1-4ac2-a9bb-d66548f42554/datasets/59c36f8c-a94a-4830-8c3d-1e17bb296f38/rows?redirectedFromSignup=1&key=XieaJ5raN4bkVsWzgEaq8SjacAAkllHhzJyvVE3rMJowxw9tD8qQ2FYVpCM4upXgHpEbm1dlPjQxgtvklvNywA%3D%3D";
                var json = JsonConvert.SerializeObject(batch);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                using var client = new HttpClient();

                var response = await client.PostAsync(url, content);

                string result = response.Content.ReadAsStringAsync().Result;
                index = index + 1;
            }
        }
    }
}
