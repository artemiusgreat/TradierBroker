using System;
using System.Threading;

namespace Tradier.Queries.MarketData
{
  public class TimeSalesRequest
  {
    public string Symbol { get; set; }
    public string Interval { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
    public string Filter { get; set; } = "all";
  }
}
