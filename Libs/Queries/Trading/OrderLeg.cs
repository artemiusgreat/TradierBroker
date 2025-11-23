using Tradier.Enums;

namespace Tradier.Queries.Trading
{
  public class OrderLeg
  {
    public int Quantity { get; set; }
    public string Type { get; set; }
    public string Symbol { get; set; }
    public string OptionSymbol { get; set; }
    public OrderSideEnum? Side { get; set; }
    public double? Price { get; set; }
    public double? Stop { get; set; }
  }
}
