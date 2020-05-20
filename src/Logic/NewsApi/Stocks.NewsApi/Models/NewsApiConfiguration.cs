namespace Stocks.NewsApi.Models
{
    public class NewsApiConfiguration
    {
        public string SandboxApiKey { get; set; }

        public string ProdApiKey { get; set; }

        public bool IsTest { get; set; }

        public string GetKey()
        {
            return this.IsTest ? this.SandboxApiKey : this.ProdApiKey;
        }
    }
}
