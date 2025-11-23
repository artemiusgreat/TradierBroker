using Flurl;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Tradier.Messages.Trading;
using Tradier.Queries;
using Tradier.Queries.Trading;

namespace Tradier
{
  public partial class TradierBroker
  {
    /// <summary>
    /// Place an order to trade a single option
    /// </summary>
    /// <param name="request">Option order request parameters</param>
    /// <param name="cleaner"></param>
    public virtual async Task<OrderResponseMessage> SendOptionOrder(OpenOrderRequest request, CancellationToken? cleaner = default)
    {
      var data = new Dictionary<string, string>
      {
        { "class", "option" },
        { "symbol", request.Symbol },
        { "option_symbol", request.OptionSymbol },
        { "side", request.Side },
        { "quantity", request.Quantity.ToString() },
        { "type", request.Type?.ToString() },
        { "duration", request.Duration?.ToString() ?? "day" },
        { "price", request.Price?.ToString() },
        { "stop", request.Stop?.ToString() },
        { "preview", request.Preview.ToString() }
      };

      var query = new Query()
      {
        Source = $"{DataUri}/accounts/{request.AccountNumber}/orders",
        Action = HttpMethod.Post,
        Content = data
      };

      var response = await Send<OrderResponseCoreMessage>(query, cleaner);

      return response.OrderReponse;
    }

    /// <summary>
    /// Place a multileg order with up to 4 legs
    /// </summary>
    /// <param name="request">Group order request parameters</param>
    /// <param name="cleaner"></param>
    public virtual async Task<OrderResponseMessage> SendGroupOrder(OpenOrderRequest request, CancellationToken? cleaner = default)
    {
      var data = new Dictionary<string, string>
      {
        { "class", "multileg" },
        { "symbol", request.Symbol },
        { "type", request.Type?.ToString() },
        { "duration", request.Duration?.ToString() ?? "day" },
        { "price", $"{request.Price}" },
        { "preview", $"{request.Preview}" }
      };

      var index = 0;

      foreach (var leg in request.Legs)
      {
        data.Add($"option_symbol[{index}]", leg.OptionSymbol);
        data.Add($"side[{index}]", leg.Side?.ToString());
        data.Add($"quantity[{index}]", leg.Quantity.ToString());
        index++;
      }

      var query = new Query()
      {
        Source = $"{DataUri}/accounts/{request.AccountNumber}/orders",
        Action = HttpMethod.Post,
        Content = data
      };

      var response = await Send<OrderResponseCoreMessage>(query, cleaner);

      return response.OrderReponse;
    }

    /// <summary>
    /// Place an order to trade an equity security
    /// </summary>
    /// <param name="request">Equity order request parameters</param>
    /// <param name="cleaner"></param>
    public virtual async Task<OrderResponseMessage> SendEquityOrder(OpenOrderRequest request, CancellationToken? cleaner = default)
    {
      var data = new Dictionary<string, string>
      {
        { "account_id", request.AccountNumber },
        { "class", "equity" },
        { "symbol", request.Symbol },
        { "side", request.Side },
        { "quantity", request.Quantity.ToString()},
        { "type", request.Type?.ToString() },
        { "duration", request.Duration?.ToString() ?? "day" },
        { "price", request.Price?.ToString() },
        { "stop", request.Stop?.ToString() },
        { "preview", request.Preview.ToString() }
      };

      var query = new Query()
      {
        Source = $"{DataUri}/accounts/{request.AccountNumber}/orders".SetQueryParams(data),
        Action = HttpMethod.Post,
        Content = data
      };

      var response = await Send<OrderResponseCoreMessage>(query, cleaner);

      return response.OrderReponse;
    }

    /// <summary>
    /// Place a combo order. This is a specialized type of order consisting of one equity leg and one option leg
    /// </summary>
    /// <param name="request">Combo order request parameters</param>
    /// <param name="cleaner"></param>
    public virtual async Task<OrderResponseMessage> SendComboOrder(OpenOrderRequest request, CancellationToken? cleaner = default)
    {
      var data = new Dictionary<string, string>
      {
        { "class", "combo" },
        { "symbol", request.Symbol },
        { "type", request.Type?.ToString() },
        { "duration", request.Duration?.ToString() ?? "day" },
        { "price", request.Price?.ToString() },
        { "preview", request.Preview.ToString() }
      };

      var index = 0;

      foreach (var leg in request.Legs)
      {
        data.Add($"option_symbol[{index}]", leg.OptionSymbol);
        data.Add($"side[{index}]", leg.Side?.ToString());
        data.Add($"quantity[{index}]", leg.Quantity.ToString());
        index++;
      }

      var query = new Query()
      {
        Source = $"{DataUri}/accounts/{request.AccountNumber}/orders".SetQueryParams(data),
        Action = HttpMethod.Post,
        Content = data
      };

      var response = await Send<OrderResponseCoreMessage>(query, cleaner);

      return response.OrderReponse;
    }

    /// <summary>
    /// Place a one-triggers-other order. This order type is composed of two separate orders sent simultaneously
    /// </summary>
    /// <param name="request">OTO order request parameters</param>
    /// <param name="cleaner"></param>
    public virtual async Task<OrderResponseMessage> SendOtoOrder(OpenOrderRequest request, CancellationToken? cleaner = default)
    {
      var data = new Dictionary<string, string>
      {
        { "class", "oto" },
        { "duration", request.Duration?.ToString() ?? "day" },
        { "preview", request.Preview.ToString() }
      };

      var index = 0;

      foreach (var leg in request.Legs)
      {
        data.Add($"symbol[{index}]", leg.Symbol);
        data.Add($"quantity[{index}]", leg.Quantity.ToString());
        data.Add($"type[{index}]", leg.Type);
        data.Add($"option_symbol[{index}]", leg.OptionSymbol);
        data.Add($"side[{index}]", leg.Side?.ToString());
        data.Add($"price[{index}]", leg.Price?.ToString() ?? "");
        data.Add($"stop[{index}]", leg.Stop?.ToString() ?? "");
        index++;
      }

      var query = new Query()
      {
        Source = $"{DataUri}/accounts/{request.AccountNumber}/orders".SetQueryParams(data),
        Action = HttpMethod.Post,
        Content = data
      };

      var response = await Send<OrderResponseCoreMessage>(query, cleaner);

      return response.OrderReponse;
    }

    /// <summary>
    /// Place a one-cancels-other order. This order type is composed of two separate orders sent simultaneously
    /// </summary>
    /// <param name="request">OCO order request parameters</param>
    /// <param name="cleaner"></param>
    public virtual async Task<OrderResponseMessage> SendOcoOrder(OpenOrderRequest request, CancellationToken? cleaner = default)
    {
      var data = new Dictionary<string, string>
      {
        { "class", "oco" },
        { "duration", request.Duration?.ToString() ?? "day" },
        { "preview", request.Preview.ToString() }
      };

      var index = 0;

      foreach (var leg in request.Legs)
      {
        data.Add($"symbol[{index}]", leg.Symbol);
        data.Add($"quantity[{index}]", leg.Quantity.ToString());
        data.Add($"type[{index}]", leg.Type);
        data.Add($"option_symbol[{index}]", leg.OptionSymbol);
        data.Add($"side[{index}]", leg.Side?.ToString());
        data.Add($"price[{index}]", leg.Price?.ToString() ?? "");
        data.Add($"stop[{index}]", leg.Stop?.ToString() ?? "");
        index++;
      }

      var query = new Query()
      {
        Source = $"{DataUri}/accounts/{request.AccountNumber}/orders".SetQueryParams(data),
        Action = HttpMethod.Post,
        Content = data
      };

      var response = await Send<OrderResponseCoreMessage>(query, cleaner);

      return response.OrderReponse;
    }

    /// <summary>
    /// Place a one-triggers-one-cancels-other order. This order type is composed of three separate orders sent simultaneously
    /// </summary>
    /// <param name="request">OTOCO order request parameters</param>
    /// <param name="cleaner"></param>
    public virtual async Task<OrderResponseMessage> SendOtocoOrder(OpenOrderRequest request, CancellationToken? cleaner = default)
    {
      var data = new Dictionary<string, string>
      {
        { "class", "otoco" },
        { "duration", request.Duration?.ToString() ?? "day" },
        { "preview", request.Preview.ToString() }
      };

      var index = 0;

      foreach (var leg in request.Legs)
      {
        data.Add($"symbol[{index}]", leg.Symbol);
        data.Add($"quantity[{index}]", leg.Quantity.ToString());
        data.Add($"type[{index}]", leg.Type);
        data.Add($"option_symbol[{index}]", leg.OptionSymbol);
        data.Add($"side[{index}]", leg.Side?.ToString());
        data.Add($"price[{index}]", leg.Price?.ToString() ?? "");
        data.Add($"stop[{index}]", leg.Stop?.ToString() ?? "");
        index++;
      }

      var query = new Query()
      {
        Source = $"{DataUri}/accounts/{request.AccountNumber}/orders".SetQueryParams(data),
        Action = HttpMethod.Post,
        Content = data
      };

      var response = await Send<OrderResponseCoreMessage>(query, cleaner);

      return response.OrderReponse;
    }

    /// <summary>
    /// Modify an order. You may change some or all of these parameters.
    /// </summary>
    /// <param name="request">Update order request parameters</param>
    /// <param name="cleaner"></param>
    public virtual async Task<OrderResponseMessage> UpdateOrder(BaseOrderRequest request, CancellationToken? cleaner = default)
    {
      var data = new Dictionary<string, string>
      {
        { "type", request.Type?.ToString() },
        { "duration", request.Duration?.ToString() ?? "day" },
        { "price", request.Price?.ToString() ?? "" },
        { "stop", request.Stop?.ToString() ?? "" },
      };

      var query = new Query()
      {
        Source = $"{DataUri}/accounts/{request.AccountNumber}/orders/{request.OrderId}".SetQueryParams(data),
        Action = HttpMethod.Put,
        Content = data
      };

      var response = await Send<OrderResponseCoreMessage>(query, cleaner);

      return response.OrderReponse;
    }

    /// <summary>
    /// Cancel an order using the default account number
    /// </summary>
    /// <param name="request">Cancel order request parameters</param>
    /// <param name="cleaner"></param>
    public virtual async Task<OrderResponseMessage> ClearOrder(BaseOrderRequest request, CancellationToken? cleaner = default)
    {
      var query = new Query()
      {
        Source = $"{DataUri}/accounts/{request.AccountNumber}/orders/{request.OrderId}",
        Action = HttpMethod.Delete,
      };

      var response = await Send<OrderResponseCoreMessage>(query);

      return response.OrderReponse;
    }
  }
}
