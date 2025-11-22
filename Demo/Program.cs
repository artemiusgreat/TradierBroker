using System;
using System.Linq;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Tradier;
using Tradier.Enums;
using Tradier.Queries.Trading;

namespace Demo
{
  public class Program
  {
    static async Task Main(string[] args)
    {
      var account = "";
      var broker = new TradierBroker { Token = "", SessionToken = "" };
      var cleaner = CancellationToken.None;

      await broker.Connect(cleaner);

      // Requests

      var maxDate = DateTime.Now;
      var minDate = DateTime.Now.AddDays(-10);
      var nextDate = DateTime.Now.AddDays(2);
      var bars = await broker.GetHistoricalQuotes(new() { Symbol = "SPY", End = maxDate, Start = minDate });
      var options = await broker.GetOptionChain(new() { Symbol = "SPY", Expiration = nextDate });
      var summary = await broker.GetBalances(new() { AccountNumber = account });
      var orders = await broker.GetOrders(new() { AccountNumber = account });
      var positions = await broker.GetPositions(new() { AccountNumber = account });

      // Subscriptions

      broker.OnPrice += o => Console.WriteLine(JsonSerializer.Serialize(o));
      broker.OnOrder += o => Console.WriteLine(JsonSerializer.Serialize(o));

      await broker.Subscribe("SPY");

      // Option order

      var optionOrder = new GroupOrderRequest
      {
        Price = 0.05,
        AccountNumber = account,
        Symbol = "SPY",
        Type = OrderTypeEnum.DEBIT,
        Legs =
        [
          new()
          {
            Quantity = 1,
            OptionSymbol = "SPY251124C00640000",
            Side = OrderSideEnum.BUY_TO_OPEN,
          },
          new()
          {
            Quantity = 1,
            OptionSymbol = "SPY251124C00680000",
            Side = OrderSideEnum.SELL_TO_OPEN,
          }
        ]
      };

      var optionResponse = await broker.SendGroupOrder(optionOrder);
      var optionStatus = await broker.ClearOrder(new() { OrderId = optionResponse.Id, AccountNumber = account });

      Console.ReadKey();

      broker.Disconnect();
    }
  }
}
