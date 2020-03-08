using System;
using Stocks.Interinio;
using Stocks.Interinio.Services;

namespace Pythia.CLI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Test securities intrinio");

            SettingsService s = new SettingsService();
            var settings = s.GetSettings();
            SecuritiesService i = new SecuritiesService(settings);
            Console.WriteLine(i.GetTestSignal("AAPL"));

            Console.WriteLine("Test fundementals intrinio");
            FundementalsService f = new FundementalsService(settings);
            Console.WriteLine(f.GetTestSignal("AAPL"));

            Console.WriteLine("Test fundementals details intrinio");
            Console.WriteLine(f.GetReportedFinancialsByTicker("AAPL-income_statement-2018-Q1"));

            Console.WriteLine("Test fundementals details, standardized intrinio");
            Console.WriteLine(f.GetStandardizedFundementals("AAPL-income_statement-2018-Q1"));

            Console.WriteLine("Test fundementals details, by company");
            Console.WriteLine(f.GetStandardizedFundementals("AAPL"));

            Console.WriteLine("Test ichimoku intrinio");
            TechnicalsService ts = new TechnicalsService(settings);
            Console.WriteLine(ts.TestSignal("AAPL"));


        }
    }
}
