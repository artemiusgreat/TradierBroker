using Tradier.Enums;

namespace Tradier.Queries.Trading
{
  public class BaseOrderRequest
  {
    public int OrderId { get; set; }
    public string AccountNumber { get; set; }
    public double? Stop { get; set; }
    public double? Price { get; set; }
    public double? Quantity { get; set; }
    public OrderTypeEnum? Type { get; set; }
    public OrderDurationEnum? Duration { get; set; }
  }
}
