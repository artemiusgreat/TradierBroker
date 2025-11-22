using System.Threading;

namespace Tradier.Queries.MarketData
{
  public class SymbolLookupRequest
  {
    public string Query { get; set; }
    public string Exchanges { get; set; }
    public string Types { get; set; }
  }
}
