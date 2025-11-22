using System.Threading;

namespace Tradier.Queries.MarketData
{
  public class SearchRequest
  {
    public string Query { get; set; }
    public bool Indexes { get; set; } = false;
  }
}
