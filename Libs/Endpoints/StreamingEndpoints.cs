using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tradier.Messages.Stream;
using Tradier.Queries;

namespace Tradier
{
  public partial class TradierBroker
  {
    /// <summary>
    /// Data stream
    /// </summary>
    /// <param name="cleaner"></param>
    public virtual async Task<SessionMessage> GetDataSession(CancellationToken cleaner)
    {
      var query = new Query()
      {
        Source = $"{SessionUri}/markets/events/session",
        Action = HttpMethod.Post,
        Token = SessionToken,
      };

      var response = await Send<SessionMessage>(query, cleaner);

      return response;
    }

    /// <summary>
    /// Order stream
    /// </summary>
    /// <param name="cleaner"></param>
    public virtual async Task<SessionMessage> GetAccountSession(CancellationToken cleaner)
    {
      var query = new Query()
      {
        Source = $"{SessionUri}/accounts/events/session",
        Action = HttpMethod.Post,
        Token = SessionToken,
      };

      var response = await Send<SessionMessage>(query, cleaner);

      return response;
    }
  }
}
