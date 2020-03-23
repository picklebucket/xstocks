using Microsoft.Extensions.DependencyInjection;
using Stocks.Tests.DI;

namespace Stocks.Tests.Fixtures
{
    public class StocksFixture
    {
        public ServiceProvider ServiceProvider { get; }

        public StocksFixture()
        {
            ServiceProvider = DIConfiguration.GetServiceProvider();
        }
    }
}
