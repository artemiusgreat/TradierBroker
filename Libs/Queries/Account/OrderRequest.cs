namespace Tradier.Queries.Account
{
  using System.Threading;

  public class OrderRequest
  {
    public int OrderId { get; set; }
    public string AccountNumber { get; set; }
  }
}
