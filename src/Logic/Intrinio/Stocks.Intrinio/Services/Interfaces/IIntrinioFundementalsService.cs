using Intrinio.SDK.Model;
using Stocks.Common.Models;
using Stocks.Intrinio.Models;
using System;
using System.Threading.Tasks;

namespace Stocks.Intrinio.Services.Interfaces
{
    public interface IIntrinioFundementalsService
    {
        string GetTestSignal(string ticker);
        string GetFundementalByTicker(string ticker);
        string GetReportedFinancialsByTicker(string valueString);
        Task<ServiceResponse<ApiResponseStandardizedFinancials>> GetStandardizedFundementalsAsync(string ticker, StatementCode statement, int year, FiscalPeriod fiscalPeriod);
        string GetFundementalsByCompany(string ticker);
    }
}
