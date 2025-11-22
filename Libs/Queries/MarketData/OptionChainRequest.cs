using System;
using System.Threading;

namespace Tradier.Queries.MarketData
{
  public class OptionChainRequest
  {
    public string Symbol { get; set; }
    public DateTime? Expiration { get; set; }
    public bool Greeks { get; set; } = true;
  }
}
