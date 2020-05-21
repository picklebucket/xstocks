using Microsoft.Extensions.DependencyInjection;
using Stocks.ML.SentimentAnalysis;
using Stocks.Tests.Fixtures;
using Xunit;
using Xunit.Abstractions;

namespace Stocks.Tests.Services
{
    [Collection(Constants.CollectionName)]
    public class SentimentAnalysisServiceTests : BaseTests
    {
        private readonly ISentimentAnalysisService _service;
        private readonly ITestOutputHelper _testOutputHelper;

        public SentimentAnalysisServiceTests(StocksFixture stocksFixture, ITestOutputHelper testOutputHelper) : base(stocksFixture)
        {
            _service = stocksFixture.ServiceProvider.GetService<ISentimentAnalysisService>();
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void Analyze()
        {
            var value = _service.Analyze("EasyJet to resume flights on June 15, starting with U.K. and France");
            Assert.NotNull(value);
            _testOutputHelper.WriteLine(value.ToString());
        }
    }
}
