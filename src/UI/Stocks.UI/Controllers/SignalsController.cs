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
    public class SyncController : ControllerBase
    {

    }

    [ApiController]
    [Route("api/[controller]")]
    public class SignalsController : ControllerBase
    {
        private readonly IReportService _reportService;

        public SignalsController(IReportService reportService)
        {
            this._reportService = reportService;
        }

        [HttpGet("Get50EMABullReport")]
        public async Task<ActionResult<List<SignalReportItem>>> Get50EMABullReport(string symbol, DateTime start, DateTime end)
        {
            return Ok(await _reportService.Build50EmaBullReportAsync(symbol, start, end, FrequencyType.Daily));
        }

        [HttpGet("Get50EMABearReport")]
        public async Task<ActionResult<List<SignalReportItem>>> Get50EMABearReport(string symbol, DateTime start, DateTime end)
        {
            return Ok(await _reportService.Build50EmaBearReportAsync(symbol, start, end, FrequencyType.Daily));
        }

        [HttpGet("Get100SMABearReport")]
        public async Task<ActionResult<List<SignalReportItem>>> Get100SMABearReport(string symbol, DateTime start, DateTime end)
        {
            return Ok(await _reportService.Build100SmaBearReportAsync(symbol, start, end, FrequencyType.Daily));
        }

        [HttpGet("Get100SMABullReport")]
        public async Task<ActionResult<List<SignalReportItem>>> Get100SMABullReport(string symbol, DateTime start, DateTime end)
        {
            return Ok(await _reportService.Build100SmaBullReportAsync(symbol, start, end, FrequencyType.Daily));
        }

        [HttpGet("GetIchiReport")]
        public async Task<ActionResult<IchiReport>> GetIchiReport(string symbol, DateTime start, DateTime end)
        {
            return Ok(await _reportService.BuildReportNewAsync(symbol, start, end, FrequencyType.Daily));
        }
    }
}
