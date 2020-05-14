using HtmlAgilityPack;
using Intrinio.SDK.Model;
using Stocks.Intrinio.Models;
using Stocks.Intrinio.Services.Interfaces;
using Stocks.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Stocks.Services
{
    public class CalculateIIntrinsicValueService : ICalculateIIntrinsicValueService
    {
        private readonly IIntrinioFundementalsService _intrinioFundementalsService;

        public CalculateIIntrinsicValueService(IIntrinioFundementalsService intrinioFundementalsService)
        {
            _intrinioFundementalsService = intrinioFundementalsService;
        }

        public async Task<decimal?> CalculateIntrinsicValueAsync(string ticker, int year, FiscalPeriod fiscalPeriod)
        {
            var cashflowResponse = await _intrinioFundementalsService
                .GetStandardizedFundementalsAsync(ticker, StatementCode.CashflowStatement, year, fiscalPeriod).ConfigureAwait(false);
            decimal? operatingCashFlow = GetTagValue(cashflowResponse.Content, "netcashfromoperatingactivities");            


            var balanceSheetResponse = await _intrinioFundementalsService
                .GetStandardizedFundementalsAsync(ticker, StatementCode.BalanceSheetStatement, year, fiscalPeriod).ConfigureAwait(false);
            decimal? longTermDebt = GetTagValue(balanceSheetResponse.Content, "longtermdebt");
            decimal? shortTermDebt = GetTagValue(balanceSheetResponse.Content, "shorttermdebt"); 
            decimal? totalDebt = longTermDebt + shortTermDebt;

            decimal? cash = GetTagValue(balanceSheetResponse.Content, "cashandequivalents");
            decimal? shortTermInvestments = GetTagValue(balanceSheetResponse.Content, "shortterminvestments");
            decimal? cashAndShortTermInvestments = cash + shortTermInvestments;

            var calculationsResponse = await _intrinioFundementalsService
                .GetStandardizedFundementalsAsync(ticker, StatementCode.Calculations, year, fiscalPeriod).ConfigureAwait(false);
            decimal? cashFlowGrowthRate = GetTagValue(calculationsResponse.Content, "fcffgrowth");
            if(cashFlowGrowthRate.HasValue)
            {
                cashFlowGrowthRate = Math.Abs(cashFlowGrowthRate.Value);
            }

            //calculate projected cashflow and discounted value for next 10 years
            decimal? projectedCashFlow = operatingCashFlow * (1 + cashFlowGrowthRate);
            decimal discountRate = 0.09M; //TODO::calculate discount rate based on BETA
            decimal? discountFactor = 1 / (1 + discountRate);
            decimal? discountedValue = projectedCashFlow * discountFactor;
            decimal? discountedValueSum = discountedValue;
            for (int i = 0; i < 9; i++)
            {
                projectedCashFlow *= (1 + cashFlowGrowthRate);
                discountFactor /= (1 + discountRate);
                discountedValue = projectedCashFlow * discountFactor;
                discountedValueSum += discountedValue;
            }

            decimal? numberOfSharesOutstanding = await GetNumberOfSharesOutstandingAsync(ticker).ConfigureAwait(false);
            decimal? intrinsicValueBeforeCashOrDebt = 0, lessDebtPerShare = 0, plusCashPerShare = 0;
            if(numberOfSharesOutstanding > 0)
            {
                intrinsicValueBeforeCashOrDebt = discountedValueSum / numberOfSharesOutstanding;
                lessDebtPerShare = totalDebt / numberOfSharesOutstanding;
                plusCashPerShare = cashAndShortTermInvestments / numberOfSharesOutstanding;
            }
            decimal? finalIntrinsicValuePerShare = intrinsicValueBeforeCashOrDebt - lessDebtPerShare + plusCashPerShare;
            return finalIntrinsicValuePerShare;
        }

        private decimal? GetTagValue(ApiResponseStandardizedFinancials financials, string tagName)
        {
            return financials.StandardizedFinancials
                .FirstOrDefault(x => x.DataTag.Tag == tagName)
                ?.Value;
        }

        private static readonly HttpClient _outstandingSharesClient = new HttpClient()
        {
            BaseAddress = new Uri("https://finviz.com/")
        };

        //temporary scrape number of shares outstanding as it is not available from api
        private async Task<decimal?> GetNumberOfSharesOutstandingAsync(string ticker)
        {
            decimal? numberOfSharesOutstanding = null;
            var response = await _outstandingSharesClient.GetAsync($"quote.ashx?t={ticker}").ConfigureAwait(false);
            if(response.IsSuccessStatusCode)
            {
                string html = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                var sharesOutstandingTitleElement = doc.DocumentNode
                    .SelectSingleNode("//table[@class=\"snapshot-table2\"]/tr[1]/td[text()=\"Shs Outstand\"]");
                var sharesOutstandingValueElement = sharesOutstandingTitleElement.NextSibling;
                string sharesOutstandingValue = sharesOutstandingValueElement.InnerText;
                char dimension = sharesOutstandingValue.Last();
                int multiplier = 1;
                if(dimension == 'B' || dimension == 'M')
                {
                    sharesOutstandingValue = sharesOutstandingValue.Substring(0, sharesOutstandingValue.Length - 1);
                    if(dimension == 'B')
                    {
                        multiplier = 1000000000;
                    }
                    else if(dimension == 'M')
                    {
                        multiplier = 1000000;
                    }
                }
                if(decimal.TryParse(sharesOutstandingValue, out decimal parsedShares))
                {
                    numberOfSharesOutstanding = parsedShares * multiplier;
                }
            }
            return numberOfSharesOutstanding;
        }
    }
}
