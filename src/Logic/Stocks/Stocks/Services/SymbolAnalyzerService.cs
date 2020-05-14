using Stocks.Common.Models;
using Stocks.Data.Entities;
using Stocks.Data.Repositories.Interfaces;
using Stocks.Intrinio.Models;
using Stocks.Intrinio.Services.Interfaces;
using Stocks.Services.Interfaces;
using System;
using System.Threading.Tasks;

namespace Stocks.Services
{
    public class SymbolAnalyzerService : ISymbolAnalyzerService
    {
        private readonly ICalculateIIntrinsicValueService _calculateIIntrinsicValueService;
        private readonly IIntrinioSecuritiesService _intrinioSecuritiesService;
        private readonly IGoodPriceCompaniesRepository _goodPriceCompaniesRepository;

        public SymbolAnalyzerService(
            ICalculateIIntrinsicValueService calculateIIntrinsicValueService,
            IIntrinioSecuritiesService intrinioSecuritiesService,
            IGoodPriceCompaniesRepository goodPriceCompaniesRepository)
        {
            this._calculateIIntrinsicValueService = calculateIIntrinsicValueService;
            this._intrinioSecuritiesService = intrinioSecuritiesService;
            this._goodPriceCompaniesRepository = goodPriceCompaniesRepository;
        }

        public async Task<ServiceResponse<bool>> AnalyzeSymbolBasedOnIntrinsicValueAsync
            (string ticker, int year, FiscalPeriod fiscalPeriod)
        {
            try
            {
                decimal? intrinsicValue = await _calculateIIntrinsicValueService.CalculateIntrinsicValueAsync(ticker, year, fiscalPeriod)
                    .ConfigureAwait(false);
                if (intrinsicValue.HasValue)
                {
                    ServiceResponse<RealtimePriceResponse> intrinoResponse =
                        await _intrinioSecuritiesService.GetRealtimeStockPriceAsync(ticker).ConfigureAwait(false);
                    if (intrinoResponse.IsSuccess)
                    {
                        if (intrinsicValue > intrinoResponse.Content.LastPrice)
                        {
                            var companyData = new GoodPriceCompany
                            {
                                DateTime = DateTime.UtcNow,
                                IntrinsicValue = intrinsicValue.Value,
                                LastTradePrice = intrinoResponse.Content.LastPrice.Value,
                                LastTradeTime = intrinoResponse.Content.LastTime.Value,
                                Symbol = ticker
                            };

                            await _goodPriceCompaniesRepository.CreateAsync(companyData).ConfigureAwait(false);
                            return ServiceResponse<bool>.Success(true);
                        }
                    }
                    else
                    {
                        return ServiceResponse<bool>.Success(false);
                    }
                }
            }
            catch (Exception ex)
            {
                return ServiceResponse<bool>.Error(ex.Message);
            }

            return ServiceResponse<bool>.Success(false);
        }
    }
}
