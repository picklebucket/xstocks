using System;
using System.Collections.Generic;
using System.Text;

namespace Stocks.Models.Api.Ichimoku
{
    public class IchimokuResponse
    {
        public List<IchimokuItem> Items { get; }

        public IchimokuResponse(List<IchimokuItem> items)
        {
            Items = items;
        }
    }
}
