using System;
namespace Stocks.Interinio.Services.Interfaces
{
    public interface IFundementalsService
    {
        string GetTestSignal(string ticker);
        string GetFundementalByTicker(string ticker);
        string GetReportedFinancialsByTicker(string valueString);
        string GetStandardizedFundementals(string valueString);
        string GetFundementalsByCompany(string ticker);
    }
}
