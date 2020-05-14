using Stocks.Data.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stocks.Data.Repositories.Interfaces
{
    public interface IGoodPriceCompaniesRepository : IBaseRepository<GoodPriceCompany>
    {
        Task DeleteAsync(string symbol);

        Task<GoodPriceCompany> GetAsync(string symbol);

        Task<IReadOnlyCollection<GoodPriceCompany>> GetForPeriod(DateTime? start = null, DateTime? end = null);
    }
}
