using System;
namespace Stocks.Intrinio.Services.Interfaces
{
    public interface ISecuritiesService
    {
        string GetTestSignal(string ticker);
    }
}
