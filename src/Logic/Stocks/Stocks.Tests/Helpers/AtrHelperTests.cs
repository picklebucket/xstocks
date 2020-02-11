using Stocks.Data.Entities;
using Stocks.Data.Repositories.Interfaces;
using Stocks.Helpers;
using Stocks.Models;
using Stocks.Services;
using Stocks.Services.Interfaces;
using Stocks.Tests.Mock;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Stocks.Tests.Helpers
{
    public class AtrHelperTests
    {
        private readonly IReportService _reportService;

        public AtrHelperTests()
        {
            _reportService = new ReportService(new FilePriceHistoryRepository());
        }

        [Fact]
        public async Task CalculateAtr()
        {
            string symbol = "NFLX";
            var candles = await _reportService.GetCollectedCandlesAsync(symbol);
            var atrItems = AtrHelper.CalculateATR(candles);
            int i;
            for (i = 0; i < 13; i++)
            {
                Assert.NotNull(atrItems[i].TR);
                Assert.Null(atrItems[i].ATR);
            }
            for (; i < atrItems.Count; i++)
            {
                Assert.NotNull(atrItems[i].TR);
                Assert.NotNull(atrItems[i].ATR);
            }
        }
    }
}
