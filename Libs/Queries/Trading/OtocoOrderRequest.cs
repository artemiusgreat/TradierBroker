using System.Collections.Generic;
using System.Threading;

namespace Tradier.Queries.Trading
{
  public class OtocoOrderRequest
  {
    public string AccountNumber { get; set; }
    public string Duration { get; set; }
    public List<AdvancedOrderLeg> Legs { get; set; }
    public bool Preview { get; set; } = false;
  }
}
