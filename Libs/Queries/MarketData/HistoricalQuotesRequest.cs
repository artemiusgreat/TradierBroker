using System;
using System.Threading;

namespace Tradier.Queries.MarketData
{
  public class HistoricalQuotesRequest
  {
    public string Symbol { get; set; }
    public string Interval { get; set; }
    public DateTime Start { get; set; }
    public DateTime End { get; set; }
  }
}
