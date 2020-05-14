using Stocks.Common.Models;
using Stocks.Intrinio.Models;
using System.Threading.Tasks;

namespace Stocks.Services.Interfaces
{
    public interface ISymbolAnalyzerService
    {
        Task<ServiceResponse<bool>> AnalyzeSymbolBasedOnIntrinsicValueAsync(string ticker, int year, FiscalPeriod fiscalPeriod);
    }
}
