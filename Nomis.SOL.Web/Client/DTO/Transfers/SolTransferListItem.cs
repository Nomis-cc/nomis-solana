using System.Text.Json.Serialization;
using Nomis.SOL.Web.Helpers;

namespace Nomis.SOL.Web.Client.DTO.Transfers
{
    public class SolTransferListItem : ITransferListItem
    {
        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [JsonPropertyName("src")]
        public string Src { get; set; }

        [JsonPropertyName("dst")]
        public string Dst { get; set; }

        [JsonPropertyName("txHash")]
        public string TxHash { get; set; }

        [JsonPropertyName("lamport")]
        public long Lamport { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("decimals")]
        public int Decimals { get; set; }

        [JsonPropertyName("txNumberSolTransfer")]
        public long TxNumberSolTransfer { get; set; }

        [JsonPropertyName("blockTime")]
        public long BlockTime { get; set; }

        public DateTime Date => BlockTime.ToDateTime();
    }
}