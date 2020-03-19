using Stocks.Common.Tests;
using Stocks.Intrinio.Models;
using Stocks.Intrinio.Services;
using Stocks.Intrinio.Services.Interfaces;
using Stocks.Intrinio.Tests.Fixtures;
using Stocks.Intrinio.Wrappers;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using Xunit;
using Xunit.Abstractions;

namespace Stocks.Intrinio.Tests.Services
{
    [Collection(TestConstants.IntrinioCollectionName)]
    public class IntrinioIchimokuServiceTests
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private readonly IntrinioSecurityApi _intrinioSecurityApi;

        public IntrinioIchimokuServiceTests(IntrinioFixture intrinioFixture, ITestOutputHelper testOutputHelper)
        {
            _intrinioSecurityApi = intrinioFixture.IntrinioSecurityApi;
            _testOutputHelper = testOutputHelper;
        }

        [Fact]
        public void GetIchimoku()
        {
            _testOutputHelper.WriteLine("Test ichimoku intrinio");
            IIntrinioIchimokuService ts = new IntrinioIchimokuService(_intrinioSecurityApi);
            var response = ts.GetIchimoku(new IchimokuRequest("AAPL", new DateTime(2018, 1, 1), new DateTime(2019, 1, 1)));
            Assert.True(response.IsSuccess, response.ErrorMessage);
            _testOutputHelper.WriteLine(response.Content.ToPrettyJson());
        }
    }
}
