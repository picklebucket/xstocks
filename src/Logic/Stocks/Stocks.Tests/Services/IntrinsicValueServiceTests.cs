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

namespace Stocks.Tests.Services
{
    [Collection(Constants.CollectionName)]

    public class IntrinsicValueServiceTests : BaseTests
    {
        private readonly IIntrinsicValueService _intrinsicValueService;

        public IntrinsicValueServiceTests(StocksFixture stocksFixture) : base(stocksFixture)
        {
            _intrinsicValueService = stocksFixture.ServiceProvider.GetService<IIntrinsicValueService>();
        }

        [Fact]
        public async Task Calculate()
        {
            await _intrinsicValueService.Calculate("AAPL", 2018, FiscalPeriod.Q1).ConfigureAwait(false);
        }
    }
}
