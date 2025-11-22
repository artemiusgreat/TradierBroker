using System.Collections.Generic;
using System.Threading;

namespace Tradier.Queries.Watchlist
{
  public class AddSymbolsRequest
  {
    public string WatchlistId { get; set; }
    public List<string> Symbols { get; set; }
  }
}
