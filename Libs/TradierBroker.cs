using Flurl.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Threading;
using System.Threading.Tasks;
using Tradier.Messages.Account;
using Tradier.Messages.Stream;
using Tradier.Queries;
using Tradier.Services;

namespace Tradier
{
  public partial class TradierBroker : IDisposable
  {
    /// <summary>
    /// Event session
    /// </summary>
    protected string dataSession;

    /// <summary>
    /// Account session
    /// </summary>
    protected string accountSession;

    /// <summary>
    /// Mapper
    /// </summary>
    protected MapService mapper = new();

    /// <summary>
    /// Web socket for events
    /// </summary>
    protected ClientWebSocket dataStreamer;

    /// <summary>
    /// Web socket for account
    /// </summary>
    protected ClientWebSocket accountStreamer;

    /// <summary>
    /// Disposable connections
    /// </summary>
    protected IList<IDisposable> connections = [];

    /// <summary>
    /// API key
    /// </summary>
    public virtual string Token { get; set; }

    /// <summary>
    /// API key for streaming
    /// </summary>
    public virtual string SessionToken { get; set; }

    /// <summary>
    /// HTTP endpoint
    /// </summary>
    public virtual string DataUri { get; set; }

    /// <summary>
    /// Socket endpoint
    /// </summary>
    public virtual string StreamUri { get; set; }

    /// <summary>
    /// Streaming authentication endpoint
    /// </summary>
    public virtual string SessionUri { get; set; }

    /// <summary>
    /// Price notification
    /// </summary>
    public virtual Action<PriceMessage> OnPrice { get; set; } = o => { };

    /// <summary>
    /// Order notification
    /// </summary>
    public virtual Action<OrderMessage> OnOrder { get; set; } = o => { };

    /// <summary>
    /// Error notification
    /// </summary>
    public virtual Action<Exception> OnError { get; set; } = o => { };

    /// <summary>
    /// Constructor
    /// </summary>
    public TradierBroker()
    {
      DataUri = "https://sandbox.tradier.com/v1";
      SessionUri = "https://api.tradier.com/v1";
      StreamUri = "wss://ws.tradier.com/v1";
    }

    /// <summary>
    /// Dispose
    /// </summary>
    public virtual void Dispose()
    {
      Disconnect();
    }

    /// <summary>
    /// Connect
    /// </summary>
    /// <param name="cleaner"></param>
    public virtual async Task Connect(CancellationToken cleaner)
    {
      var dataStreamer = new ClientWebSocket();
      var accountStreamer = new ClientWebSocket();

      this.dataStreamer = dataStreamer;
      this.accountStreamer = accountStreamer;
      this.dataSession = (await GetDataSession(cleaner))?.Stream?.Session;
      this.accountSession = (await GetAccountSession(cleaner)).Stream?.Session;

      await CreateConnection("/markets/events", dataStreamer, message =>
      {
        var messageType = $"{message["type"]}";

        switch (messageType)
        {
          case "quote": OnPrice(message.Deserialize<PriceMessage>(mapper.Options)); break;
          case "trade": break;
          case "tradex": break;
          case "summary": break;
          case "timesale": break;
        }
      });

      await CreateConnection("/accounts/events", accountStreamer, message =>
      {
        OnOrder(message.Deserialize<OrderMessage>(mapper.Options));
      });

      connections.Add(dataStreamer);
      connections.Add(accountStreamer);
    }

    /// <summary>
    /// Save state and dispose
    /// </summary>
    public virtual void Disconnect()
    {
      OnPrice = o => { };
      OnOrder = o => { };

      connections?.ForEach(o => o?.Dispose());
      connections?.Clear();
    }

    /// <summary>
    /// Subscribe to data streams
    /// </summary>
    /// <param name="names"></param>
    public virtual async Task Subscribe(params string[] names)
    {
      var dataMessage = new DataMessage
      {
        Symbols = [.. names],
        Filter = ["trade", "quote", "summary", "timesale", "tradex"],
        Session = dataSession
      };

      var accountMessage = new Messages.Stream.AccountMessage
      {
        Events = ["order"],
        Session = accountSession
      };

      await SendStream(dataStreamer, dataMessage);
      await SendStream(accountStreamer, accountMessage);
    }

    /// <summary>
    /// Send data to the API
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="query"></param>
    /// <param name="cleaner"></param>
    public virtual async Task<T> Send<T>(Query query, CancellationToken? cleaner = default)
    {
      var message = $"{new UriBuilder(query.Source)}"
        .WithHeader("Accept", "application/json")
        .WithHeader("Authorization", $"Bearer {query.Token ?? Token}");

      var data = null as FormUrlEncodedContent;

      if (query.Content.Count is not 0)
      {
        data = new FormUrlEncodedContent(query.Content);
      }

      var response = await message
        .SendAsync(query.Action, data, HttpCompletionOption.ResponseContentRead, cleaner ?? CancellationToken.None)
        .ConfigureAwait(false);

      foreach (var o in query.Headers ?? [])
      {
        query.Headers[o.Key] = response.ResponseMessage.Headers.TryGetValues(o.Key, out var v) ? v : null;
      }

      var responseContent = await response
        .ResponseMessage
        .Content
        .ReadAsStringAsync()
        .ConfigureAwait(false);

      return JsonSerializer.Deserialize<T>(responseContent.Replace("\"null\"", "null"), mapper.Options);
    }

    /// <summary>
    /// Send data to web socket stream
    /// </summary>
    /// <param name="streamer"></param>
    /// <param name="data"></param>
    /// <param name="cleaner"></param>
    protected virtual Task SendStream(ClientWebSocket streamer, object data, CancellationTokenSource cleaner = null)
    {
      var content = JsonSerializer.Serialize(data, mapper.Options);
      var message = Encoding.UTF8.GetBytes(content);

      return streamer.SendAsync(
        message,
        WebSocketMessageType.Text,
        true,
        cleaner?.Token ?? CancellationToken.None);
    }

    /// <summary>
    /// Web socket stream
    /// </summary>
    /// <param name="uri"></param>
    /// <param name="streamer"></param>
    /// <param name="action"></param>
    protected virtual async Task CreateConnection(string uri, ClientWebSocket streamer, Action<JsonNode> action, CancellationTokenSource cleaner = null)
    {
      var data = new byte[short.MaxValue];
      var source = new UriBuilder($"{StreamUri}{uri}");

      await streamer.ConnectAsync(source.Uri, cleaner.Token);

      var process = new Thread(async o =>
      {
        while (streamer.State is WebSocketState.Open)
        {
          try
          {
            var streamResponse = await streamer.ReceiveAsync(new ArraySegment<byte>(data), cleaner.Token);
            var content = Encoding.UTF8.GetString(data, 0, streamResponse.Count);

            action(JsonNode.Parse(content));
          }
          catch (Exception e)
          {
            OnError(e);
          }
        }
      });

      process.Start();
    }
  }
}
