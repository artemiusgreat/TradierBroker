using Tradier.Enums;

namespace Tradier.Queries.Trading
{
  public class OrderLeg
  {
    public string OptionSymbol { get; set; }
    public OrderSideEnum? Side { get; set; }
    public int Quantity { get; set; }
  }
}
