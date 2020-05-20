using Stocks.Data.Entities;
using System.Threading.Tasks;

namespace Stocks.Data.Repositories.Interfaces
{
    public interface INewsDetailsRepository : IBaseRepository<NewsDetails>
    {
        Task Update(NewsDetails news);
    }
}
