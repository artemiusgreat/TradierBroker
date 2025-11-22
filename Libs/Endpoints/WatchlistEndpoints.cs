using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tradier.Messages.Watchlist;
using Tradier.Queries;
using Tradier.Queries.Watchlist;

namespace Tradier
{
  /// <summary>
  /// The <c>Watchlist</c> class
  /// </summary>
  public partial class TradierBroker
  {
    /// <summary>
    /// Retrieve all of a users watchlists
    /// </summary>
    /// <param name="request">Watchlist request parameters</param>
    /// <param name="cleaner"></param>
    public virtual async Task<WatchlistsMessage> GetWatchlists(WatchlistByIdRequest request, CancellationToken? cleaner = default)
    {
      var query = new Query()
      {
        Source = $"{DataUri}/watchlists",
      };

      var response = await Send<WatchlistsCoreMessage>(query, cleaner);

      return response.Watchlists;
    }

    /// <summary>
    /// Retrieve a specific watchlist by id
    /// </summary>
    /// <param name="request">Watchlist by ID request parameters</param>
    /// <param name="cleaner"></param>
    public virtual async Task<WatchlistMessage> GetWatchlist(WatchlistByIdRequest request, CancellationToken? cleaner = default)
    {
      var query = new Query()
      {
        Source = $"{DataUri}/watchlists/{request.WatchlistId}",
      };

      var response = await Send<WatchlistCoreMessage>(query, cleaner);

      return response.Watchlist;
    }

    /// <summary>
    /// Create a new watchlist
    /// </summary>
    /// <param name="request">Create watchlist request parameters</param>
    /// <param name="cleaner"></param>
    public virtual async Task<WatchlistMessage> CreateWatchlist(CreateWatchlistRequest request, CancellationToken? cleaner = default)
    {
      var data = new Dictionary<string, string>
      {
        { "name", request.Name },
        { "symbols", string.Join(",", request.Symbols) },
      };

      var query = new Query()
      {
        Source = $"{DataUri}/watchlists",
        Action = HttpMethod.Post,
        Content = data
      };

      var response = await Send<WatchlistCoreMessage>(query, cleaner);

      return response.Watchlist;
    }

    /// <summary>
    /// Update an existing watchlist
    /// </summary>
    /// <param name="request">Update watchlist request parameters</param>
    /// <param name="cleaner"></param>
    public virtual async Task<WatchlistMessage> UpdateWatchlist(UpdateWatchlistRequest request, CancellationToken? cleaner = default)
    {
      var data = new Hashtable
      {
        { "name", request.Name },
        { "symbols", string.Join(",", request.Symbols ?? new List<string>()) },
      };

      var query = new Query()
      {
        Source = $"{DataUri}/watchlists/{request.WatchlistId}",
        Action = HttpMethod.Put,
      };

      var response = await Send<WatchlistCoreMessage>(query, cleaner);

      return response.Watchlist;
    }

    /// <summary>
    /// Delete a specific watchlist
    /// </summary>
    /// <param name="request">Delete watchlist request parameters</param>
    /// <param name="cleaner"></param>
    public virtual async Task<WatchlistsMessage> DeleteWatchlist(DeleteWatchlistRequest request, CancellationToken? cleaner = default)
    {
      var query = new Query()
      {
        Source = $"{DataUri}/watchlists/{request.WatchlistId}",
        Action = HttpMethod.Delete,
      };

      var response = await Send<WatchlistsCoreMessage>(query, cleaner);

      return response.Watchlists;
    }

    /// <summary>
    /// Add symbols to an existing watchlist. If the symbol exists, it will be over-written
    /// </summary>
    /// <param name="request">Add symbols request parameters</param>
    /// <param name="cleaner"></param>
    public virtual async Task<WatchlistMessage> AddSymbolsToWatchlist(AddSymbolsRequest request, CancellationToken? cleaner = default)
    {
      var data = new Dictionary<string, string>
      {
        { "symbols", string.Join(",", request.Symbols) },
      };

      var query = new Query()
      {
        Source = $"{DataUri}/watchlists/{request.WatchlistId}/symbols",
        Action = HttpMethod.Post,
        Content = data
      };

      var response = await Send<WatchlistCoreMessage>(query, cleaner);

      return response.Watchlist;
    }

    /// <summary>
    /// Remove a symbol from a specific watchlist
    /// </summary>
    /// <param name="request">Remove symbol request parameters</param>
    /// <param name="cleaner"></param>
    public virtual async Task<WatchlistMessage> RemoveSymbolFromWatchlist(RemoveSymbolRequest request, CancellationToken? cleaner = default)
    {
      var query = new Query()
      {
        Source = $"{DataUri}/watchlists/{request.WatchlistId}/symbols/{request.Symbol}",
        Action = HttpMethod.Delete,
      };

      var response = await Send<WatchlistCoreMessage>(query, cleaner);

      return response.Watchlist;
    }
  }
}
