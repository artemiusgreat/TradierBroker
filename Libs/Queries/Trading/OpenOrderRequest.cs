using System.Collections.Generic;
using Tradier.Enums;

namespace Tradier.Queries.Trading
{
  public class OpenOrderRequest : BaseOrderRequest
  {
    public bool Preview { get; set; } = false;
    public string Symbol { get; set; }
    public string OptionSymbol { get; set; }
    public OrderSideEnum? Side { get; set; }
    public List<OpenOrderRequest> Legs { get; set; } = [];
  }
}
