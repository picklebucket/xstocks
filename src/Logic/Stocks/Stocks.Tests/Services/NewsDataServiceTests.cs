using Microsoft.Extensions.DependencyInjection;
using Stocks.Services;
using Stocks.Tests.Fixtures;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Stocks.Tests.Services
{
    [Collection(Constants.CollectionName)]
    public class NewsDataServiceTests : BaseTests
    {
        private readonly INewsDataService _newsDataService;
        private readonly ITestOutputHelper _testOutputHelper;

        public NewsDataServiceTests(StocksFixture stocksFixture, ITestOutputHelper testOutputHelper) : base(stocksFixture)
        {
            _newsDataService = stocksFixture.ServiceProvider.GetService<INewsDataService>();
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task SyncNews()
        {
            var value = await _newsDataService.SyncNews("us").ConfigureAwait(false);
            Assert.NotNull(value);
            _testOutputHelper.WriteLine(value.ToString());
        }
    }
}
