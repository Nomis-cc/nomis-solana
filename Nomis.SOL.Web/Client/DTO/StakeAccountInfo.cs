using System.Text.Json.Serialization;

namespace Nomis.SOL.Web.Client.DTO;

public class StakeAccountInfo
{
    [JsonPropertyName("amount")]
    public string Amount { get; set; }
}