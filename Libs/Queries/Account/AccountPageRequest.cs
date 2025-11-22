namespace Tradier.Queries.Account
{
  using System.Threading;

  public class AccountPageRequest
  {
    public int Page { get; set; } = 1;
    public int ItemsPerPage { get; set; } = 25;
    public string AccountNumber { get; set; }
  }
}
