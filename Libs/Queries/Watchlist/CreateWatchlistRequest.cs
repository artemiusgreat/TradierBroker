using System.Collections.Generic;
using System.Threading;

namespace Tradier.Queries.Watchlist
{
  public class CreateWatchlistRequest
  {
    public string Name { get; set; }
    public List<string> Symbols { get; set; }
  }
}
