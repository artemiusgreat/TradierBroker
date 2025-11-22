using System.Threading;

namespace Tradier.Queries.MarketData
{
  public class OptionExpirationsRequest
  {
    public string Symbol { get; set; }
    public bool? IncludeRoots { get; set; } = true;
    public bool? Strikes { get; set; } = true;
  }
}
