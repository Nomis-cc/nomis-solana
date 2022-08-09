using System.Text.Json.Serialization;
using Nomis.SOL.Web.Helpers;

namespace Nomis.SOL.Web.Client.DTO;

public class TransactionListItem
{
    [JsonPropertyName("lamport")]
    public long Lamport { get; set; }

    [JsonPropertyName("txHash")]
    public string TxHash { get; set; }

    [JsonPropertyName("blockTime")]
    public long BlockTime { get; set; }

    public DateTime Date => BlockTime.ToDateTime();

    [JsonPropertyName("status")]
    public string Status { get; set; }

    [JsonPropertyName("parsedInstruction")]
    public List<TransactionListItemParsedInstruction> ParsedInstructions { get; set; } = new();
}