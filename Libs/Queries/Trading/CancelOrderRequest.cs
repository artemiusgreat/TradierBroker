using System.Threading;

namespace Tradier.Queries.Trading
{
  public class CancelOrderRequest
  {
    public string AccountNumber { get; set; }
    public int OrderId { get; set; }
  }
}
