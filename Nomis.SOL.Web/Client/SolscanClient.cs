using Nomis.SOL.Web.Client.DTO;
using System.Text.Json;
using System.Text.Json.Nodes;
using Nomis.SOL.Web.Client.DTO.Transfers;
using Nomis.SOL.Web.Helpers;

namespace Nomis.SOL.Web.Client;

public class SolscanClient
{
    private const int ItemsFetchLimit = 50;

    private readonly HttpClient _client;

    public SolscanClient(Uri baseUri)
    {
        _client = new()
        {
            BaseAddress = baseUri
        };
    }

    public async Task<decimal> GetBalance(string address)
    {
        var response = await _client.GetAsync($"/account/{address}");
        response.EnsureSuccessStatusCode();
        var accountInfo = await response.Content.ReadFromJsonAsync<AccountInfo>();
        return accountInfo?.Lamports.ToSol() ?? 0;
    }

    private async Task<TransactionListItem[]> GetTxList(string address, string beforeHash = null)
    {
        var request =
            $"/account/transactions?account={address}&limit={ItemsFetchLimit}";
        if (!string.IsNullOrWhiteSpace(beforeHash))
        {
            request = $"{request}&beforeHash={beforeHash}";
        }

        var response = await _client.GetAsync(request);
        response.EnsureSuccessStatusCode();
        var transactionsData = await response.Content.ReadFromJsonAsync<TransactionListItem[]>();
        return transactionsData;
    }

    public async Task<IEnumerable<TransactionListItem>> GetTransactionsData(string address)
    {
        var result = new List<TransactionListItem>();
        var transactionsData = await GetTxList(address);
        result.AddRange(transactionsData);
        while (transactionsData.Length >= ItemsFetchLimit)
        {
            transactionsData = await GetTxList(address, transactionsData.Last().TxHash);
            result.AddRange(transactionsData);
        }

        return result;
    }

    private async Task<TResult> GetTransfersList<TResult, TResultItem>(string address, long? fromTime = null, long? toTime = null, int? offset = null)
        where TResult : ITransferList<TResultItem>
        where TResultItem : ITransferListItem
    {
        var request = "/account";
        if (typeof(TResult) == typeof(SplTransferList))
        {
            request = $"{request}/splTransfers";
        }
        else if (typeof(TResult) == typeof(SolTransferList))
        {
            request = $"{request}/solTransfers";
        }
        else
        {
            return default;
        }

        request = $"{request}?account={address}&limit={ItemsFetchLimit}";

        if (fromTime != null)
        {
            request = $"{request}&fromTime={fromTime}";
        }
        if (toTime != null)
        {
            request = $"{request}&toTime={toTime}";
        }
        request = $"{request}&offset={offset ?? 0}";

        var response = await _client.GetAsync(request);
        response.EnsureSuccessStatusCode();
        var transactionsData = await response.Content.ReadFromJsonAsync<TResult>();
        return transactionsData;
    }

    public async Task<TResult> GetTransfersData<TResult, TResultItem>(string address) 
        where TResult : ITransferList<TResultItem>, new()
        where TResultItem : ITransferListItem
    {
        var result = new TResult
        {
            Data = new()
        };
        var offset = 0;
        var transactionsData = await GetTransfersList<TResult, TResultItem>(address);
        result.Data.AddRange(transactionsData.Data);
        while (transactionsData.Data.Count > 0)
        {
            offset += ItemsFetchLimit;
            transactionsData = await GetTransfersList<TResult, TResultItem>(address, offset: offset);
            result.Data.AddRange(transactionsData.Data);
        }

        return result;
    }

    public async Task<IEnumerable<TokenListItem>> GetTokens(string address)
    {
        var response = await _client.GetAsync($"/account/tokens?account={address}");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<TokenListItem[]>();
        return result;
    }

    public async Task<IEnumerable<StakeAccountInfo>> GetStakes(string address)
    {
        var response = await _client.GetAsync($"/account/stakeAccounts?account={address}");
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadFromJsonAsync<JsonObject>();
        var accounts = result?
            .Where(x => x.Value != null)
            .Select(x => JsonSerializer.Deserialize<StakeAccountInfo>(x.Value.ToJsonString()));

        return accounts;
    }
}