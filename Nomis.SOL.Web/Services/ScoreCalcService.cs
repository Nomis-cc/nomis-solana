using System.Net;
using Solnet.Wallet;

namespace Nomis.SOL.Web.Services;

using Client;
using Client.DTO.Transfers;
using Helpers;

public class ScoreCalcService
{
    private readonly SolscanClient _client;
    private readonly MagicEdenClient _magicEdenClient;
    private readonly ILogger<ScoreCalcService> _logger;

    public ScoreCalcService(
        SolscanClient solscanClient,
        MagicEdenClient magicEdenClient,
        ILogger<ScoreCalcService> logger)
    {
        _client = solscanClient;
        _magicEdenClient = magicEdenClient;
        _logger = logger;
    }

    private IEnumerable<double> GetTransactionsIntervals(IEnumerable<DateTime> transactionDates)
    {
        var result = new List<double>();
        DateTime? lasDateTime = null;
        foreach (var transactionDate in transactionDates)
        {
            if (!lasDateTime.HasValue)
            {
                lasDateTime = transactionDate;
                continue;
            }

            var interval = Math.Abs((transactionDate - lasDateTime.Value).TotalHours);
            result.Add(interval);
        }

        return result;
    }

    private long GetMinBlockTime(params long[] blockTimes)
    {
        var result = long.MaxValue;
        foreach (var blockTime in blockTimes)
        {
            result = Math.Min(result, blockTime);
        }

        return result;
    }

    private long GetMaxBlockTime(params long[] blockTimes)
    {
        var result = long.MinValue;
        foreach (var blockTime in blockTimes)
        {
            result = Math.Max(result, blockTime);
        }

        return result;
    }

    public async Task<WalletScoreResult> GetStatsAsync(string address)
    {
        try
        {
            var result = new WalletScoreResult();

            var stats = new WalletStats();

            try
            {
                var publicKey = new PublicKey(address);
                if (!publicKey.IsValid())
                {
                    throw new InvalidAddressException("Invalid address");
                }
            }
            catch (ArgumentException e)
            {
                throw new InvalidAddressException(e.Message);
            }

            var magicEdenWalletActivities = (await _magicEdenClient.GetWalletActivitiesData(address)).ToList();
            var magicEdenWalletBuys = magicEdenWalletActivities.Where(x => x.Type?.Equals("buyNow") == true && x.Buyer?.Equals(address) == true).ToList();
            var magicEdenWalletSells = magicEdenWalletActivities.Where(x => x.Type?.Equals("buyNow") == true && x.Seller?.Equals(address) == true).ToList();

            var splTransfers = await _client.GetTransfersData<SplTransferList, SplTransferListItem>(address);
            var solTransfers = await _client.GetTransfersData<SolTransferList, SolTransferListItem>(address);

            var splTransferIntervals = GetTransactionsIntervals(splTransfers.Data.Select(x => x.Date)).ToList();
            var solTransferIntervals = GetTransactionsIntervals(solTransfers.Data.Select(x => x.Date)).ToList();
            var allTransferIntervals = splTransferIntervals.Union(solTransferIntervals).ToList();

            if (!allTransferIntervals.Any())
            {
                throw new NoDataException("There is no transfers for this wallet");
            }

            var monthAgo = DateTime.Now.AddMonths(-1);

            //var soldTokens = solTransfers.Data.Where(x => x.Src?.Equals(address) == true).ToList();
            //var soldSum = soldTokens.Sum(x => x.Lamport).ToSol();

            //var buyTokens = solTransfers.Data.Where(x => x.Dst?.Equals(address) == true).ToList();
            //var buySum = buyTokens.Sum(x => x.Lamport).ToSol();

            var soldSplTokens = splTransfers.Data.Where(x => x.ChangeType == "dec");
            var soldSplNftTokens = soldSplTokens.Where(x => x.Decimals == 0).ToList();
            var soldSplNftSum =
                Math.Abs(soldSplNftTokens.Sum(x =>
                    x.ChangeAmount)); // TODO - не считает сумму, т.к. в данных от API нет её
            var soldSplNftTokensIds = soldSplNftTokens.Select(x => x.Address);

            var buySplTokens = splTransfers.Data.Where(x => x.ChangeType == "inc");
            var buySplNftTokens = buySplTokens.Where(x => x.Decimals == 0).ToList();
            var buySplNftSum = buySplNftTokens.Where(x => soldSplNftTokensIds.Contains(x.Address))
                .Sum(x => x.ChangeAmount); // TODO - не считает сумму, т.к. в данных от API нет её

            var buyNotSoldSplNftTokens = buySplNftTokens.Where(x => !soldSplNftTokensIds.Contains(x.Address));
            var buyNotSoldSplNftSum = buyNotSoldSplNftTokens.Sum(x => x.ChangeAmount);

            var holdingNftTokens = splTransfers.Data.Count(x => x.Decimals == 0) - soldSplNftTokens.Count;
            var nftWorth = buySplNftSum == 0
                ? 0
                : (decimal)soldSplNftSum / (decimal)buySplNftSum * (decimal)buyNotSoldSplNftSum;

            var accountTokens = (await _client.GetTokens(address)).ToList();

            stats.Balance = await _client.GetBalance(address);
            stats.WalletAge =
                (int)((DateTime.UtcNow - GetMinBlockTime(splTransfers.Data.Min(x => x.BlockTime),
                    solTransfers.Data.Any() ? solTransfers.Data.Min(x => x.BlockTime) : long.MaxValue).ToDateTime()).TotalDays / 30);
            stats.TotalTransactions = splTransfers.Total + solTransfers.Data.Count;
            stats.MinTransactionTime = allTransferIntervals.Min();
            stats.MaxTransactionTime = allTransferIntervals.Max();
            stats.AverageTransactionTime = allTransferIntervals.Average();
            stats.WalletTurnover = solTransfers.Data.Sum(x => x.Lamport).ToSol();
            stats.LastMonthTransactions = splTransfers.Data.Count(x => x.Date > monthAgo)
                                          + solTransfers.Data.Count(x => x.Date > monthAgo);
            stats.TimeFromLastTransaction =
                (int)((DateTime.UtcNow - GetMaxBlockTime(splTransfers.Data.Min(x => x.BlockTime),
                    solTransfers.Data.Any() ? solTransfers.Data.Min(x => x.BlockTime) : long.MinValue).ToDateTime()).TotalDays / 30);
            stats.NftHolding =
                accountTokens.Count(x => x.TokenAmount.Decimals == 0 && x.TokenAmount.UiAmount > 0); //holdingNftTokens;
            stats.NftTrading = magicEdenWalletSells.Sum(x => x.Price) - magicEdenWalletBuys.Sum(x => x.Price); //(soldSplNftSum - buySplNftSum);
            stats.NftWorth = magicEdenWalletBuys.Where(x => !magicEdenWalletSells.Select(y => y.TokenMint).Contains(x.TokenMint)).Sum(x => x.Price); //nftWorth;
            stats.TokensHolding = accountTokens.Count(x => x.TokenAmount.UiAmount > 0);

            // TODO - превышает лимит по запросам, так как очень много транзакций может быть
            /*var transactions = await _client.GetTransactionsData(address);
            stats.DeployedContracts = transactions.Where(x => x.Status.Equals("success")).SelectMany(x => x.ParsedInstructions)
                .Count(x => x.Type.Equals("bpf-loader: Finalize"));*/

            result.Stats = stats;
            result.Score = stats.GetScore();
            return result;
        }
        catch (HttpRequestException e)
        {
            _logger.LogError(e, "Http request exception for address = {Address}.", address);
            if (e.StatusCode.Equals(HttpStatusCode.BadRequest))
            {
                throw new InvalidAddressException("Invalid address");
            }

            throw;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error gathering stats for address = {Address}.", address);
            throw;
        }
    }
}