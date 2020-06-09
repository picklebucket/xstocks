using Microsoft.AspNetCore.Mvc;
using Stocks.Models;
using Stocks.Models.Api;
using Stocks.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stocks.UI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SignalsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public SignalsController(IReportService reportService)
        {
            this._reportService = reportService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Ema50ReportItem>>> GetSignalsReport(string symbol, DateTime start, DateTime end)
        {
            return Ok(await _reportService.Build50EmaReportAsync(symbol, start, end, FrequencyType.Daily));
        }

        //[HttpGet("roc")]
        //public async Task<ActionResult<List<Ema50ReportItem>>> GetRocReport(string symbol, DateTime start, DateTime end)
        //{
        //    return Ok(await _reportService.R(symbol, start, end, FrequencyType.Daily));
        //}
    }
}
