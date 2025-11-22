using System.Threading;

namespace Tradier.Queries.Watchlist
{
  public class RemoveSymbolRequest
  {
    public string WatchlistId { get; set; }
    public string Symbol { get; set; }
  }
}
