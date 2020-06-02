using Stocks.Common.Models;
using System.Threading.Tasks;

namespace Stocks.Services
{
    public interface INewsDataService
    {
        Task<ServiceResponse> SyncNews(string query);
    }
}
