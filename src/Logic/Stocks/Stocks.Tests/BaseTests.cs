using Microsoft.Extensions.DependencyInjection;
using Stocks.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Tests
{
    public abstract class BaseTests
    {
        protected ServiceProvider ServiceProvider { get; }

        protected BaseTests(StocksFixture stocksFixture)
        {
            ServiceProvider = stocksFixture.ServiceProvider;
        }
    }
}
