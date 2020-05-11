using Stocks.Intrinio.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Services.Interfaces
{
    public interface IIntrinsicValueService
    {
        Task Calculate(string ticker, int year, FiscalPeriod fiscalPeriod);
    }
}
