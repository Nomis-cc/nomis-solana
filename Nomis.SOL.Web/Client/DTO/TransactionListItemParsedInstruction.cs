using System.Text.Json.Serialization;

namespace Nomis.SOL.Web.Client.DTO
{
    public class TransactionListItemParsedInstruction
    {
        [JsonPropertyName("programId")]
        public string ProgramId { get; set; }

        [JsonPropertyName("program")]
        public string Program { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }
    }
}