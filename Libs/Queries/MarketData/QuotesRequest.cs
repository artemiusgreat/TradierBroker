using System.Collections.Generic;
using System.Threading;

namespace Tradier.Queries.MarketData
{
  public class QuotesRequest
  {
    public IList<string> Symbols { get; set; }
    public bool Greeks { get; set; } = true;
  }
}
