using System.Text.Json.Serialization;

namespace Nomis.SOL.Web.Client.DTO;

public class AccountInfo
{
    [JsonPropertyName("lamports")]
    public long Lamports { get; set; }
}