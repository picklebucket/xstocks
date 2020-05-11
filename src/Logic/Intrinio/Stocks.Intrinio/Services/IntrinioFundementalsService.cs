using System;
using System.Linq;
using System.Threading.Tasks;
using Intrinio.SDK.Api;
using Intrinio.SDK.Client;
using Intrinio.SDK.Model;
using Stocks.Common.Models;
using Stocks.Intrinio.Models;
using Stocks.Intrinio.Services.Interfaces;
using Stocks.Intrinio.Wrappers;

namespace Stocks.Intrinio.Services
{
    public class IntrinioFundementalsService : IIntrinioFundementalsService
    {
        private readonly IntrinioFundamentalsApi _intrinioFundamentalsApi;

        public IntrinioFundementalsService(IntrinioFundamentalsApi intrinioFundamentalsApi)
        {
            _intrinioFundamentalsApi = intrinioFundamentalsApi;
        }
        public string GetTestSignal(string ticker)
        {
            var identifier = ticker;  // string | A Company identifier (Ticker, CIK, LEI, Intrinio ID)
            var statementCode = "cash_flow_statement";  // string | The statement code
            var fiscalYear = 2017;  // int? | The fiscal year
            var fiscalPeriod = "FY";  // string | The fiscal period

            try
            {
                string response = null;
                var a = _intrinioFundamentalsApi.GetFundamentalStandardizedFinancials("AAPL-balance_sheet_statement-2018-Q1");
                response = a.ToJson();
                //Fundamental result = _intrinioFundamentalsApi.LookupFundamental(identifier, statementCode, fiscalYear, fiscalPeriod);
                //response = result.ToString();
                return response;
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ticker">normal security ticker symbol all caps</param>
        /// <returns></returns>
        public string GetFundementalByTicker(string ticker)
        {

            var fundamentalsApi = new FundamentalsApi();
            var identifier = ticker;  // string | A Company identifier (Ticker, CIK, LEI, Intrinio ID)
            var statementCode = "income_statement";  // string | The statement code
            var fiscalYear = 2017;  // int? | The fiscal year
            var fiscalPeriod = "FY";  // string | The fiscal period

            try
            {
                Fundamental result = fundamentalsApi.LookupFundamental(identifier, statementCode, fiscalYear, fiscalPeriod);
                return result.ToString();
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="valueString">should look like AAPL-income_statement-2018-Q1</param>
        /// <returns></returns>
        public string GetReportedFinancialsByTicker(string valueString)
        {

            var fundamentalsApi = new FundamentalsApi();
            var id = valueString;  // string | The Intrinio ID or lookup code (ticker-statement-year-period) for the Fundamental

            try
            {
                ApiResponseReportedFinancials result = fundamentalsApi.GetFundamentalReportedFinancials(id);
                return result.ToJson();
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
        /// <summary>
        ///
        ///
        /// </summary>
        /// <param name="valueString">should look like AAPL-income_statement-2018-Q1</param>
        /// <returns></returns>
        public async Task<ServiceResponse<ApiResponseStandardizedFinancials>> GetStandardizedFundementalsAsync(string ticker, StatementCode statement, int year, FiscalPeriod fiscalPeriod)
        {
            string statementString;
            switch (statement)
            {
                case StatementCode.CashflowStatement:
                    statementString = "cash_flow_statement";
                    break;
                case StatementCode.BalanceSheetStatement:
                    statementString = "balance_sheet_statement";
                    break;
                default:
                    throw new ArgumentException("Unknown statement", nameof(statement));
            }

            string fiscalPeriodString;
            switch (fiscalPeriod)
            {
                case FiscalPeriod.Q1:
                    fiscalPeriodString = "Q1";
                    break;
                default:
                    throw new ArgumentException("Unknown fiscal period", nameof(fiscalPeriod));
            }
            
            var id = string.Join("-", ticker, statementString, year, fiscalPeriodString);  // string | The Intrinio ID or lookup code (ticker-statement-year-period) for the Fundamental

            try
            {
                ApiResponseStandardizedFinancials result = await _intrinioFundamentalsApi.GetFundamentalStandardizedFinancialsAsync(id).ConfigureAwait(false);
                return ServiceResponse<ApiResponseStandardizedFinancials>.Success(result);
            }
            catch (Exception e)
            {
                return ServiceResponse<ApiResponseStandardizedFinancials>.Error(e.Message);
            }
        }
        public string GetFundementalsByCompany(string ticker)
        {
            //throws error from the service probably on the 'fiscalyear' property. this is sample code so it should work

            var companyApi = new CompanyApi();
            var identifier = ticker;  // string | A Company identifier (Ticker, CIK, LEI, Intrinio ID)
            var filedAfter = DateTime.Now;  // DateTime? | Filed on or after this date (optional) 
            var filedBefore = DateTime.Now;  // DateTime? | Filed on or before this date (optional) 
            var reportedOnly = false;  // bool? | Only as-reported fundamentals (optional) 
            //var fiscalYear = "";  // int? | Only for the given fiscal year (optional) 
            var statementCode = "";  // string | Only of the given statement code (optional) 
            var type = "";  // string | Only of the given type (optional) 
            var startDate = DateTime.Now;  // DateTime? | Only on or after the given date (optional) 
            var endDate = DateTime.Now;  // DateTime? | Only on or before the given date (optional) 
            var pageSize = 100;  // int? | The number of results to return (optional)  (default to 100)
            var nextPage = "";  // string | Gets the next page of data from a previous API call (optional) 

            try
            {
                ApiResponseCompanyFundamentals result = companyApi.GetCompanyFundamentals(identifier, filedAfter, filedBefore, reportedOnly, 0, statementCode, type, startDate, endDate, pageSize, nextPage);
                return result.ToJson();
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }
    }
}
