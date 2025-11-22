using Flurl;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Tradier.Messages.Account;
using Tradier.Queries;
using Tradier.Queries.Account;

namespace Tradier
{
  /// <summary>
  /// The <c>Account</c> class. 
  /// </summary>
  public partial class TradierBroker
  {
    /// <summary>
    /// The user's profile contains information pertaining to the user and his/her accounts
    /// </summary>
    /// <param name="request"></param>
    /// <param name="cleaner"></param>
    public virtual async Task<ProfileMessage> GetUserProfile(AccountRequest request, CancellationToken? cleaner = default)
    {
      var query = new Query()
      {
        Source = $"{DataUri}/user/profile",
      };

      var response = await Send<ProfileCoreMessage>(query, cleaner);

      return response.Profile;
    }

    /// <summary>
    /// Get balances information for a specific or a default user account.
    /// </summary>
    /// <param name="request">Account request parameters</param>
    /// <param name="cleaner"></param>
    public virtual async Task<BalanceMessage> GetBalances(AccountRequest request, CancellationToken? cleaner = default)
    {
      var query = new Query()
      {
        Source = $"{DataUri}/accounts/{request.AccountNumber}/balances",
      };

      var response = await Send<BalanceCoreMessage>(query, cleaner);

      return response.Balance;
    }

    /// <summary>
    /// Get the current positions being held in an account. These positions are updated intraday via trading
    /// </summary>
    /// <param name="request">Account request parameters</param>
    /// <param name="cleaner"></param>
    public virtual async Task<IList<PositionMessage>> GetPositions(AccountRequest request, CancellationToken? cleaner = default)
    {
      var query = new Query()
      {
        Source = $"{DataUri}/accounts/{request.AccountNumber}/positions",
      };

      var response = await Send<PositionsCoreMessage>(query, cleaner);

      return response.Positions?.Items;
    }

    /// <summary>
    /// Get historical activity for an account
    /// </summary>
    /// <param name="request">Paginated account request parameters</param>
    /// <param name="cleaner"></param>
    public virtual async Task<HistoryMessage> GetHistory(AccountPageRequest request, CancellationToken? cleaner = default)
    {
      var data = new Hashtable
      {
        { "page", request.Page },
        { "limit", request.ItemsPerPage },
      };

      var query = new Query()
      {
        Source = $"{DataUri}/accounts/{request.AccountNumber}/history".SetQueryParams(data),
      };

      var response = await Send<HistoryCoreMessage>(query, cleaner);

      return response.History;
    }

    /// <summary>
    /// Get cost basis information for a specific user account
    /// </summary>
    /// <param name="request">Paginated account request parameters</param>
    /// <param name="cleaner"></param>
    public virtual async Task<GainLossMessage> GetGainLoss(AccountPageRequest request, CancellationToken? cleaner = default)
    {
      var data = new Hashtable
      {
        { "page", request.Page },
        { "limit", request.ItemsPerPage },
      };

      var query = new Query()
      {
        Source = $"{DataUri}/accounts/{request.AccountNumber}/gainloss".SetQueryParams(data),
      };

      var response = await Send<GainLossCoreMessage>(query, cleaner);

      return response.GainLoss;
    }

    /// <summary>
    /// Retrieve orders placed within an account
    /// </summary>
    /// <param name="request">Account request parameters</param>
    /// <param name="cleaner"></param>
    public virtual async Task<IList<OrderMessage>> GetOrders(AccountRequest request, CancellationToken? cleaner = default)
    {
      var query = new Query()
      {
        Source = $"{DataUri}/accounts/{request.AccountNumber}/orders",
      };

      var response = await Send<OrdersCoreMessage>(query, cleaner);

      return response.Orders?.Items;
    }

    /// <summary>
    /// Get detailed information about a previously placed order
    /// </summary>
    /// <param name="request">Order request parameters</param>
    /// <param name="cleaner"></param>
    public virtual async Task<OrderMessage> GetOrder(OrderRequest request, CancellationToken? cleaner = default)
    {
      var query = new Query()
      {
        Source = $"{DataUri}/accounts/{request.AccountNumber}/orders/{request.OrderId}",
      };

      var response = await Send<OrdersCoreMessage>(query, cleaner);

      return response.Orders?.Items?.FirstOrDefault();
    }
  }
}
