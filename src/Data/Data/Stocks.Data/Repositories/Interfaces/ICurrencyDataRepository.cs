using Stocks.Data.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Stocks.Data.Repositories.Interfaces
{
    public interface ICurrencyDataRepository : IBaseRepository<CurrencyData>
    {
        Task<CurrencyData> GetPairData(string pair);

        Task WipeData(string pair);
    }
}
