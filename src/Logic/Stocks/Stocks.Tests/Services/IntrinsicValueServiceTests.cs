using Microsoft.Extensions.DependencyInjection;
using Stocks.Helpers;
using Stocks.Intrinio.Models;
using Stocks.Intrinio.Services;
using Stocks.Intrinio.Services.Interfaces;
using Stocks.Services.Interfaces;
using Stocks.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace Stocks.Tests.Services
{
    [Collection(Constants.CollectionName)]

    public class IntrinsicValueServiceTests : BaseTests
    {
        private readonly IIntrinsicValueService _intrinsicValueService;
        private readonly ITestOutputHelper _testOutputHelper;

        public IntrinsicValueServiceTests(StocksFixture stocksFixture, ITestOutputHelper testOutputHelper) : base(stocksFixture)
        {
            _intrinsicValueService = stocksFixture.ServiceProvider.GetService<IIntrinsicValueService>();
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public async Task Calculate()
        {
            var value = await _intrinsicValueService.CalculateIntrinsicValueAsync("AAPL", 2018, FiscalPeriod.Q1).ConfigureAwait(false);
            Assert.NotNull(value);
            _testOutputHelper.WriteLine(value.ToString());
        }
    }
}
