using System.Collections.Generic;
using System.Threading;
using Tradier.Enums;

namespace Tradier.Queries.Trading
{
  public class GroupOrderRequest
  {
    public string AccountNumber { get; set; }
    public string Symbol { get; set; }
    public OrderTypeEnum? Type { get; set; }
    public OrderDurationEnum? Duration { get; set; }
    public List<OrderLeg> Legs { get; set; }
    public double? Price { get; set; }
    public bool Preview { get; set; } = false;
  }
}
