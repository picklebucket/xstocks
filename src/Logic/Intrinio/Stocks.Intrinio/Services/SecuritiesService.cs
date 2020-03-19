using System;
using System.Globalization;
using Intrinio.SDK.Api;
using Intrinio.SDK.Model;
using Stocks.Intrinio.Models;
using Stocks.Intrinio.Services.Interfaces;
using Stocks.Intrinio.Wrappers;

namespace Stocks.Intrinio.Services
{
    public class SecuritiesService : ISecuritiesService
    {
        private readonly IntrinioSecurityApi _intrinioSecurityApi;

        public SecuritiesService(IntrinioSecurityApi intrinioSecurityApi)
        {
            _intrinioSecurityApi = intrinioSecurityApi;
        }
        public string GetTestSignal(string ticker)
        {
            var identifier = ticker;  // string | A Security identifier (Ticker, FIGI, ISIN, CUSIP, Intrinio ID)
            var startDate = DateTime.Parse("2018-01-01", CultureInfo.InvariantCulture);  // DateTime? | Return prices on or after the date (optional) 
            var endDate = DateTime.Parse("2019-01-01", CultureInfo.InvariantCulture);  // DateTime? | Return prices on or before the date (optional) 
            var frequency = "daily";  // string | Return stock prices in the given frequency (optional)  (default to daily)
            var pageSize = 100;  // int? | The number of results to return (optional)  (default to 100)
            var nextPage = "";  // string | Gets the next page of data from a previous API call (optional) 

            try
            {
                ApiResponseSecurityStockPrices result = _intrinioSecurityApi.GetSecurityStockPrices(identifier, startDate, endDate, frequency, pageSize, nextPage);

                return result.ToString();
                //Console.WriteLine(result.ToJson());

            }
            catch (Exception e)
            {
                //Console.WriteLine("Exception when calling SecurityApi.GetSecurityStockPrices: " + e.Message);
                return e.Message;
            }
        }
    }
}
