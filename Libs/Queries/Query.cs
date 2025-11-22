namespace Tradier.Queries
{
    using System.Collections.Generic;
    using System.Net.Http;

    public class Query
  {
    public string Token { get; set; }
    public string Source { get; set; }
    public HttpMethod Action { get; set; } = HttpMethod.Get;
    public Dictionary<string, string> Content { get; set; } = new();
    public Dictionary<string, IEnumerable<string>> Headers { get; set; } = new();
  }
}
