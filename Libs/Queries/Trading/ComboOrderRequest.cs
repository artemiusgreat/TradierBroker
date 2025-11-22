using System.Collections.Generic;
using System.Threading;

namespace Tradier.Queries.Trading
{
  public class ComboOrderRequest
  {
    public string AccountNumber { get; set; }
    public string Symbol { get; set; }
    public string Type { get; set; }
    public string Duration { get; set; }
    public List<OrderLeg> Legs { get; set; }
    public double? Price { get; set; }
    public bool Preview { get; set; } = false;
  }
}
