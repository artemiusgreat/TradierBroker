using System.Threading;

namespace Tradier.Queries.Trading
{
  public class UpdateOrderRequest
  {
    public string AccountNumber { get; set; }
    public int OrderId { get; set; }
    public string Type { get; set; }
    public string Duration { get; set; }
    public double? Price { get; set; }
    public double? Stop { get; set; }
  }
}
