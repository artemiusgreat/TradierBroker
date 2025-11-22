using System.Threading;

namespace Tradier.Queries.Trading
{
  public class OptionOrderRequest
  {
    public string AccountNumber { get; set; }
    public string Symbol { get; set; }
    public string OptionSymbol { get; set; }
    public string Side { get; set; }
    public int Quantity { get; set; }
    public string Type { get; set; }
    public string Duration { get; set; }
    public double? Price { get; set; }
    public double? Stop { get; set; }
    public bool Preview { get; set; } = false;
  }
}
