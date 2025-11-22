using System;
using System.Threading;

namespace Tradier.Queries.MarketData
{
  public class StrikesRequest
  {
    public string Symbol { get; set; }
    public DateTime Expiration { get; set; }
  }
}
