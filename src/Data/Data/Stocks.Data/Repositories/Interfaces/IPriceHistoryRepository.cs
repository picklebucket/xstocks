using Stocks.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Data.Repositories.Interfaces
{
    public interface IPriceHistoryRepository : IBaseRepository<PriceHistory>
    {
        Task DeleteAsync(string symbol);
    }
}
