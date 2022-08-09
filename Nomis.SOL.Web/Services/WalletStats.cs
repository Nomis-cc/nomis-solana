namespace Nomis.SOL.Web.Services;

using Swashbuckle.AspNetCore.Annotations;

public class WalletStats
{
    [SwaggerSchema("Wallet balance (SOL)", ReadOnly = true)]
    public decimal Balance { get; set; }

    [SwaggerSchema("Wallet age (months)", ReadOnly = true)]
    public int WalletAge { get; set; }

    [SwaggerSchema("Total transactions on wallet (number)", ReadOnly = true)]
    public int TotalTransactions { get; set; }

    [SwaggerSchema("Average time interval between transactions (hours)", ReadOnly = true)]
    public double AverageTransactionTime { get; set; }

    [SwaggerSchema("Maximum time interval between transactions (hours)", ReadOnly = true)]
    public double MaxTransactionTime { get; set; }

    [SwaggerSchema("Minimal time interval between transactions (hours)", ReadOnly = true)]
    public double MinTransactionTime { get; set; }

    [SwaggerSchema("The movement of funds on the wallet(SOL)", ReadOnly = true)]
    public decimal WalletTurnover { get; set; }

    [SwaggerSchema("Total NFTs on wallet (number)", ReadOnly = true)]
    public int NftHolding { get; set; }

    [SwaggerSchema("Time since last transaction (months)", ReadOnly = true)]
    public int TimeFromLastTransaction { get; set; }

    [SwaggerSchema("NFT trading activity (SOL)", ReadOnly = true)]
    public decimal NftTrading { get; set; }

    [SwaggerSchema("NFT worth on wallet (SOL)", ReadOnly = true)]
    public decimal NftWorth { get; set; }

    [SwaggerSchema("Average transaction per months (number)", ReadOnly = true)]
    public double TransactionsPerMonth =>  (double) TotalTransactions / WalletAge;

    [SwaggerSchema("Last month transactions (number)", ReadOnly = true)]
    public int LastMonthTransactions { get; set; }

    [SwaggerSchema("Value of all holding tokens (SOL)", ReadOnly = true)]
    public int TokensHolding { get; set; }
}