using Stocks.Common.Models;
using Stocks.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stocks.Services.Interfaces
{
    public interface ICurrencyDataService
    {
        Task<ServiceResponse<CurrencyData>> SyncDataFor7days15minsInterval(string pairCode);

        Task<ServiceResponse<CurrencyData>> GetPairData(string pairData);

        Task ReportToExcel(string pairCode);

        Task ReportToExcel(List<string> pairCodes);
    }
}
