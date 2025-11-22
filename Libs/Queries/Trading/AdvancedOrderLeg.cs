namespace Tradier.Queries.Trading
{
  public class AdvancedOrderLeg
  {
    public string Symbol { get; set; }
    public int Quantity { get; set; }
    public string Type { get; set; }
    public string OptionSymbol { get; set; }
    public string Side { get; set; }
    public double? Price { get; set; }
    public double? Stop { get; set; }
  }
}
