using Flurl;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Tradier.Messages.MarketData;
using Tradier.Queries;
using Tradier.Queries.MarketData;

namespace Tradier
{

  public partial class TradierBroker
  {
    /// <summary>
    /// Get all quotes in an option chain
    /// </summary>
    /// <param name="request">Option chain request parameters</param>
    /// <param name="cleaner"></param>
    public virtual async Task<OptionsMessage> GetOptionChain(OptionChainRequest request, CancellationToken? cleaner = default)
    {
      var data = new Hashtable
      {
        { "symbol", request.Symbol },
        { "expiration", $"{request.Expiration:yyyy-MM-dd}" },
        { "greeks", request.Greeks }
      };

      var query = new Query()
      {
        Source = $"{DataUri}/markets/options/chains".SetQueryParams(data),
      };

      var response = await Send<OptionChainCoreMessage>(query, cleaner);

      return response.Options;
    }

    /// <summary>
    /// Get expiration dates for a particular underlying
    /// </summary>
    /// <param name="request">Option expirations request parameters</param>
    /// <param name="cleaner"></param>
    public virtual async Task<ExpirationsMessage> GetOptionExpirations(OptionExpirationsRequest request, CancellationToken? cleaner = default)
    {
      var data = new Hashtable
      {
        { "symbol", request.Symbol },
        { "includeAllRoots", request.IncludeRoots },
        { "strikes", request.Strikes }
      };

      var query = new Query()
      {
        Source = $"{DataUri}/markets/options/expirations".SetQueryParams(data),
      };

      var response = await Send<OptionExpirationsCoreMessage>(query, cleaner);

      return response.Expirations;
    }

    /// <summary>
    /// Get a list of symbols using a keyword lookup on the symbols description
    /// </summary>
    /// <param name="request">Quotes request parameters</param>
    /// <param name="cleaner"></param>
    public virtual async Task<QuotesMessage> GetQuotes(QuotesRequest request, CancellationToken? cleaner = default)
    {
      var data = new Hashtable
      {
        { "symbols", string.Join(",", request.Symbols) },
        { "greeks", request.Greeks }
      };

      var query = new Query()
      {
        Source = $"{DataUri}/markets/quotes".SetQueryParams(data),
      };

      var response = await Send<QuotesCoreMessage>(query, cleaner);

      return response.Quotes;
    }

    /// <summary>
    /// Get historical pricing for a security
    /// </summary>
    /// <param name="request">Historical quotes request parameters</param>
    /// <param name="cleaner"></param>
    public virtual async Task<HistoricalQuotesMessage> GetHistoricalQuotes(HistoricalQuotesRequest request, CancellationToken? cleaner = default)
    {
      var data = new Hashtable
      {
        { "symbol", request.Symbol },
        { "interval", request.Interval },
        { "start", $"{request.Start:yyyy-MM-dd}" },
        { "end", $"{request.End:yyyy-MM-dd}" }
      };

      var query = new Query()
      {
        Source = $"{DataUri}/markets/history".SetQueryParams(data),
      };

      var response = await Send<HistoricalQuotesCoreMessage>(query, cleaner);

      return response.History;
    }

    /// <summary>
    /// Get an options strike prices for a specified expiration date
    /// </summary>
    /// <param name="request">Strikes request parameters</param>
    /// <param name="cleaner"></param>
    public virtual async Task<StrikesMessage> GetStrikes(StrikesRequest request, CancellationToken? cleaner = default)
    {
      var data = new Hashtable
      {
        { "symbol", request.Symbol },
        { "expiration", $"{request.Expiration:yyyy-MM-dd}" }
      };

      var query = new Query()
      {
        Source = $"{DataUri}/markets/options/strikes".SetQueryParams(data),
      };

      var response = await Send<OptionStrikesCoreMessage>(query, cleaner);

      return response.Strikes;
    }

    /// <summary>
    /// Time and Sales (timesales) is typically used for charting purposes. It captures pricing across a time slice at predefined intervals.
    /// </summary>
    /// <param name="request">Time sales request parameters</param>
    /// <param name="cleaner"></param>
    public virtual async Task<SeriesMessage> GetTimeSales(TimeSalesRequest request, CancellationToken? cleaner = default)
    {
      var data = new Hashtable
      {
        { "symbol", request.Symbol },
        { "interval", request.Interval },
        { "session_filter", request.Filter },
        { "start", $"{request.Start:yyyy-MM-dd HH:mm}" },
        { "end", $"{request.End:yyyy-MM-dd HH:mm}" }
      };

      var query = new Query()
      {
        Source = $"{DataUri}/markets/timesales".SetQueryParams(data),
      };

      var response = await Send<TimeSalesCoreMessage>(query, cleaner);

      return response.Series;
    }

    /// <summary>
    /// The ETB list contains securities that are able to be sold short with a Tradier Brokerage account.
    /// </summary>
    /// <param name="cleaner">Cancellation token</param>
    public virtual async Task<SecuritiesMessage> GetEtbSecurities(CancellationToken cleaner)
    {
      var query = new Query()
      {
        Source = $"{DataUri}/markets/etb",
      };

      var response = await Send<SecuritiesCoreMessage>(query, cleaner);

      return response.Securities;
    }

    /// <summary>
    /// Get market clock information
    /// </summary>
    /// <param name="cleaner">Cancellation token</param>
    /// <param name="cleaner"></param>
    public virtual async Task<ClockMessage> GetClock(CancellationToken cleaner)
    {
      var query = new Query()
      {
        Source = $"{DataUri}/markets/clock",
      };

      var response = await Send<ClockCoreMessage>(query, cleaner);

      return response.Clock;
    }

    /// <summary>
    /// Get the market calendar for the current or given month
    /// </summary>
    /// <param name="request">Calendar request parameters</param>
    /// <param name="cleaner"></param>
    public virtual async Task<CalendarMessage> GetCalendar(CalendarRequest request, CancellationToken? cleaner = default)
    {
      var data = new Hashtable
      {
        { "month", request.Month },
        { "year", request.Year }
      };

      var query = new Query()
      {
        Source = $"{DataUri}/markets/calendar".SetQueryParams(data),
      };

      var response = await Send<CalendarCoreMessage>(query, cleaner);

      return response.Calendar;
    }

    /// <summary>
    /// Search for companies
    /// </summary>
    /// <param name="request">Search request parameters</param>
    /// <param name="cleaner"></param>
    public virtual async Task<SecuritiesMessage> SearchCompanies(SearchRequest request, CancellationToken? cleaner = default)
    {
      var data = new Hashtable
      {
        { "q", request.Query },
        { "indexes", request.Indexes }
      };

      var query = new Query()
      {
        Source = $"{DataUri}/markets/search".SetQueryParams(data),
      };

      var response = await Send<SecuritiesCoreMessage>(query, cleaner);

      return response.Securities;
    }

    /// <summary>
    /// Search for a symbol using the ticker symbol or partial symbol
    /// </summary>
    /// <param name="request">Symbol lookup request parameters</param>
    /// <param name="cleaner"></param>
    public virtual async Task<SecuritiesMessage> LookupSymbol(SymbolLookupRequest request, CancellationToken? cleaner = default)
    {
      var source = $"{DataUri}/markets/lookup?q={request.Query}";

      source += string.IsNullOrEmpty(request.Exchanges) ? string.Empty : $"&exchanges={request.Exchanges}";
      source += string.IsNullOrEmpty(request.Types) ? string.Empty : $"&types={request.Types}";

      var query = new Query()
      {
        Source = source,
      };

      var response = await Send<SecuritiesCoreMessage>(query, cleaner);

      return response.Securities;
    }

    /// <summary>
    /// Get all options symbols for the given underlying
    /// </summary>
    /// <param name="request">Option symbols request parameters</param>
    /// <param name="cleaner"></param>
    public virtual async Task<List<SymbolMessage>> LookupOptionSymbols(OptionSymbolsRequest request, CancellationToken? cleaner = default)
    {
      var data = new Hashtable
      {
        { "underlying", request.Symbol }
      };

      var query = new Query()
      {
        Source = $"{DataUri}/markets/options/lookup".SetQueryParams(data),
      };

      var response = await Send<OptionSymbolsCoreMessage>(query, cleaner);

      return response.Symbols;
    }

    /// <summary>
    /// Get company fundamentals data
    /// </summary>
    /// <param name="request">Company data request parameters</param>
    /// <param name="cleaner"></param>
    public virtual async Task<List<CompanyDataMessage>> GetCompany(CompanyDataRequest request, CancellationToken? cleaner = default)
    {
      var data = new Hashtable
      {
        { "symbols", request.Symbols }
      };

      var query = new Query()
      {
        Source = $"{DataUri}/beta/markets/fundamentals/company".SetQueryParams(data),
      };

      var response = await Send<CompanyDataCoreMessage>(query, cleaner);

      return response.Results;
    }

    /// <summary>
    /// Get corporate calendars data
    /// </summary>
    /// <param name="request">Corporate calendars request parameters</param>
    /// <param name="cleaner"></param>
    public virtual async Task<List<CorporateCalendarDataMessage>> GetCorporateCalendars(CorporateCalendarsRequest request, CancellationToken? cleaner = default)
    {
      var data = new Hashtable
      {
        { "symbols", request.Symbols }
      };

      var query = new Query()
      {
        Source = $"{DataUri}/beta/markets/fundamentals/calendars".SetQueryParams(data),
      };

      var response = await Send<CorporateCalendarCoreMessage>(query, cleaner);

      return response.Results;
    }
  }
}
