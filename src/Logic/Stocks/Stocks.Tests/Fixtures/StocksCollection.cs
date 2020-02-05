using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Stocks.Tests.Fixtures
{
    [CollectionDefinition(Constants.CollectionName)]
    public class StocksCollection : ICollectionFixture<StocksFixture>, ICollectionFixture<HelpersFixture>
    {
    }
}
