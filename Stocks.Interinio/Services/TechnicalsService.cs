using System;
using Intrinio.SDK.Api;
using Intrinio.SDK.Model;
using Stocks.Interinio.Models;
using Stocks.Interinio.Services.Interfaces;

namespace Stocks.Interinio.Services
{
    public class TechnicalsService : IntrinioApiBase, ITechnicalsService
    {
        public TechnicalsService(SettingsModel settings) : base(settings)
        {
        }
        public string TestSignal(string ticker)
        {
            var securityApi = new SecurityApi();
            var identifier = "AAPL";  // string | A Security identifier (Ticker, FIGI, ISIN, CUSIP, Intrinio ID)
            var lowPeriod = 9;  // int? | The number of observations, per period, to calculate Tenkan Sen (Conversion Line) of Ichimoku Kinko Hyo (optional)  (default to 9)
            var mediumPeriod = 26;  // int? | The number of observations, per period, to calculate Kijun Sen (Base Line), Senkou Span A (Leading Span A), and Chikou Span (Lagging Span) of Ichimoku Kinko Hyo (optional)  (default to 26)
            var highPeriod = 52;  // int? | The number of observations, per period, to calculate Senkou Span B (Leading Span B) of Ichimoku Kinko Hyo (optional)  (default to 52)
            var startDate = "2018-01-01";  // string | Return technical indicator values on or after the date (optional) 
            var endDate = "2019-01-01";  // string | Return technical indicator values on or before the date (optional) 
            var pageSize = 100;  // int? | The number of results to return (optional)  (default to 100)
            var nextPage = "";  // string | Gets the next page of data from a previous API call (optional) 

            try
            {
                ApiResponseSecurityIchimokuKinkoHyo result = securityApi.GetSecurityPriceTechnicalsIchimoku(identifier, lowPeriod, mediumPeriod, highPeriod, startDate, endDate, pageSize, nextPage);
                return result.ToJson();
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

    }
}
