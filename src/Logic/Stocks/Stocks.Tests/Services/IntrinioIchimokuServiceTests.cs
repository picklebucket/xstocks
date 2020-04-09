using Stocks.Services.Ichimoku;
using Stocks.Tests.Mock;
using System;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;
using Stocks.Tests.Fixtures;
using Stocks.Intrinio.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Stocks.Models.Api.Ichimoku;
using Stocks.Models.Api;
using System.Linq;

namespace Stocks.Tests.Helpers
{
    [Collection(Constants.CollectionName)]
    public class IntrinioIchimokuServiceTests : BaseTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IIchimokuService _ichimokuService;

        public IntrinioIchimokuServiceTests(ITestOutputHelper testOutputHelper, StocksFixture stocksFixture)
            :base(stocksFixture)
        {
            _ichimokuService = new IntrinioIchimokuService(ServiceProvider.GetService<IIntrinioSecuritiesService>());
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task BuildIchimokuCloud()
        {
            var startDate = new DateTime(2018, 1, 1);
            var endDate = new DateTime(2019, 1, 1);
            var request = new IchimokuRequest("AAPL", startDate, endDate, FrequencyType.Daily);
            var serviceResponse = await _ichimokuService.BuildIchimokuCloudAsync(request).ConfigureAwait(false);
            Assert.True(serviceResponse.IsSuccess, serviceResponse.ErrorMessage);
            var ordered = serviceResponse.Content.Items.OrderBy(x => x.Date).ToList();
            var lastDate = endDate.AddDays(-1);
            Assert.Equal(lastDate, ordered.Last().Date.Value.DateTime);
            var cloudItems = IchimokuItemWrapper.Create(serviceResponse.Content.Items);
            _testOutputHelper.WriteLine(JsonSerializer.Serialize(cloudItems, new JsonSerializerOptions() { WriteIndented = true}));            
        }
    }


}
