using Stocks.Helpers;
using Stocks.Models;
using Stocks.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace Stocks.Tests.Helpers
{
    public class EmaHelperTests
    {
        private readonly ITestOutputHelper _testOutputHelper;

        public EmaHelperTests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void CalculateEma()
        {
            var candles = new List<CandleItem>();
            var random = new Random();
            for (int i = 0; i < 100; i++)
            {
                candles.Add(new CandleItem() { Close = (decimal)(random.Next(1, 150) * random.NextDouble()) });
            }
            int periods = 50;
            var emaList = EmaHelper.CalculateEma(candles, periods).ToList();
            Assert.NotNull(emaList);
            for (int i = 0; i < periods - 1; i++)
            {
                Assert.Equal(default, emaList[i].Ema);
            }
            for (int i = periods - 1; i < candles.Count; i++)
            {
                Assert.NotEqual(default, emaList[i].Ema);
            }

            string json = JsonSerializer.Serialize(emaList, new JsonSerializerOptions() { WriteIndented = true });
            _testOutputHelper.WriteLine(json);
        }
    }
}
