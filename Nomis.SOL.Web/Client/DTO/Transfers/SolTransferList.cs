using System.Text.Json.Serialization;

namespace Nomis.SOL.Web.Client.DTO.Transfers
{
    public class SolTransferList : ITransferList<SolTransferListItem>
    {
        [JsonPropertyName("data")]
        public List<SolTransferListItem> Data { get; set; } = new();
    }
}