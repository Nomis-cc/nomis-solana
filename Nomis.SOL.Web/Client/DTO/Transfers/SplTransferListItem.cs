using System.Text.Json.Serialization;
using Nomis.SOL.Web.Helpers;

namespace Nomis.SOL.Web.Client.DTO.Transfers
{
    public class SplTransferListItem : ITransferListItem
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("changeType")]
        public string ChangeType { get; set; }

        [JsonPropertyName("changeAmount")]
        public long ChangeAmount { get; set; }

        [JsonPropertyName("decimals")]
        public int Decimals { get; set; }

        [JsonPropertyName("postBalance")]
        public string PostBalance { get; set; }

        [JsonPropertyName("preBalance")]
        public long PreBalance { get; set; }

        [JsonPropertyName("tokenAddress")]
        public string TokenAddress { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("blockTime")]
        public long BlockTime { get; set; }

        public DateTime Date => BlockTime.ToDateTime();

        [JsonPropertyName("balance")]
        public SplTransferListItemBalance Balance { get; set; }
    }
}

public class SplTransferListItemBalance
{
    [JsonPropertyName("amount")]
    public string Amount { get; set; }

    [JsonPropertyName("decimals")]
    public int Decimals { get; set; }
}