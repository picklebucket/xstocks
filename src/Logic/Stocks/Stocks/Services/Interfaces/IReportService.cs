using Stocks.Data.Entities;
using Stocks.Models;
using Stocks.Models.Api;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Services.Interfaces
{
    public interface IReportService
    {
        Task<IEnumerable<ReportItem>> BuildReportAsync(string symbol, DateTime startDate, DateTime endDate, FrequencyType frequencyType);
        IEnumerable<ReportItem> BuildReport(string symbol, List<IchimokuItem> ichimokuItems, List<CandleItem> candleItems);
        byte[] ExportToIndicatorCsv(IEnumerable<ReportItem> reportItems);
        byte[] ExportToTradeCsv(IEnumerable<ReportItem> reportItems);
        Task<IReadOnlyCollection<GoodPriceCompany>> BuildReportBasedOnIntrinsicValue(DateTime? startDate = null, DateTime? endDate = null);
        byte[] ExportGoodPriceCompaniesToCsv(IReadOnlyCollection<GoodPriceCompany> goodPriceCompanies);
        Task<List<SignalReportItem>> Build50EmaBullReportAsync(string symbol, DateTime startDate, DateTime endDate, FrequencyType frequencyType);
        Task Build50EmaCSVReportAsync(string symbol, DateTime startDate, DateTime endDate, FrequencyType frequencyType);
        Task<List<SignalReportItem>> Build50EmaBearReportAsync(string symbol, DateTime startDate, DateTime endDate, FrequencyType frequencyType);
        Task<List<SignalReportItem>> Build100SmaBearReportAsync(string symbol, DateTime startDate, DateTime endDate, FrequencyType frequencyType);
        Task<List<SignalReportItem>> Build100SmaBullReportAsync(string symbol, DateTime startDate, DateTime endDate, FrequencyType frequencyType);
    }
}
