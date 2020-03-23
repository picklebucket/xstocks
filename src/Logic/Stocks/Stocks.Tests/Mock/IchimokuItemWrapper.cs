using Stocks.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stocks.Tests.Mock
{
    public class IchimokuItemWrapper : IchimokuItem
    {
        public int Number { get; set; }

        public IchimokuItemWrapper(int number, DateTimeOffset? date, decimal? tenkanSen, decimal? kijunSen, decimal? chikouSpan,
            decimal? senkouSpanA, decimal? senkouSpanB) :
            base(date, tenkanSen, kijunSen, chikouSpan, senkouSpanA, senkouSpanB)
        {
            Number = number;
        }

        public static List<IchimokuItemWrapper> Create(List<IchimokuItem> items)
        {
            return items.Select((x, i) => new IchimokuItemWrapper(i + 1, x.Date, x.TenkanSen, x.KijunSen, x.ChikouSpan, x.SenkouSpanA, x.SenkouSpanB)).ToList();
        }
    }
}
