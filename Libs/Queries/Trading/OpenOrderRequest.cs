using System.Collections.Generic;

namespace Tradier.Queries.Trading
{
  public class OpenOrderRequest : BaseOrderRequest
  {
    public string Side { get; set; }
    public string Symbol { get; set; }
    public string OptionSymbol { get; set; }
    public bool Preview { get; set; } = false;
    public List<OrderLeg> Legs { get; set; } = [];
  }
}
