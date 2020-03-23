using Stocks.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Services.Interfaces
{
    public interface IReportService
    {
        IEnumerable<ReportItem> BuildReport(string symbol, List<CandleItem> candleItems);
        byte[] ExportToIndicatorCsv(IEnumerable<ReportItem> reportItems);
        byte[] ExportToTradeCsv(IEnumerable<ReportItem> reportItems);
    }
}
