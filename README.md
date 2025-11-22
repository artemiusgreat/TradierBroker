# Tradier API wrapper

Minimalistic async wrapper around Tradier API. 
A part of a trading framework [Terminal](https://github.com/Indemos/Terminal).

# Status 

![GitHub Workflow Status (with event)](https://img.shields.io/github/actions/workflow/status/Indemos/Terminal/dotnet.yml?event=push)
![GitHub](https://img.shields.io/github/license/Indemos/Terminal)
![GitHub](https://img.shields.io/badge/system-Windows%20%7C%20Linux%20%7C%20Mac-blue)

# Nuget 

`dotnet add package TradierBroker`

# Usage 

```C#

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

```