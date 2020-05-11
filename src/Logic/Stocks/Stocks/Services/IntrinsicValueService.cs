using Intrinio.SDK.Model;
using Stocks.Intrinio.Models;
using Stocks.Intrinio.Services.Interfaces;
using Stocks.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Helpers
{
    public class IntrinsicValueService : IIntrinsicValueService
    {
        private readonly IIntrinioFundementalsService _intrinioFundementalsService;

        public IntrinsicValueService(IIntrinioFundementalsService intrinioFundementalsService)
        {
            _intrinioFundementalsService = intrinioFundementalsService;
        }

        public async Task Calculate(string ticker, int year, FiscalPeriod fiscalPeriod)
        {
            var response = await _intrinioFundementalsService
                .GetStandardizedFundementalsAsync(ticker, StatementCode.CashflowStatement, year, fiscalPeriod).ConfigureAwait(false);
            decimal? operatingCashFlow = response.Content.StandardizedFinancials
                .FirstOrDefault(x => x.DataTag.Tag == "netcashfromoperatingactivities")
                ?.Value;            


            response = await _intrinioFundementalsService
                .GetStandardizedFundementalsAsync(ticker, StatementCode.BalanceSheetStatement, year, fiscalPeriod).ConfigureAwait(false);
            decimal? longTermDebt = response.Content.StandardizedFinancials
                .FirstOrDefault(x => x.DataTag.Tag == "longtermdebt")
                ?.Value;
            decimal? shortTermDebt = response.Content.StandardizedFinancials
                .FirstOrDefault(x => x.DataTag.Tag == "shorttermdebt")
                ?.Value;
            decimal? totalDebt = longTermDebt + shortTermDebt;

        }
    }
}
