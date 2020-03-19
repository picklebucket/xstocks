using Stocks.Intrinio.Tests.Fixtures;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Stocks.Intrinio.Tests.Collections
{
    [CollectionDefinition(TestConstants.IntrinioCollectionName)]
    public class IntrinioCollection : ICollectionFixture<IntrinioFixture>
    {

    }
}
