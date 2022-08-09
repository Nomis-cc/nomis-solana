using System.Text.Json.Serialization;

namespace Nomis.SOL.Web.Client.DTO.Transfers
{
    public class SplTransferList : ITransferList<SplTransferListItem>
    {
        [JsonPropertyName("total")]
        public int Total { get; set; }

        [JsonPropertyName("data")] 
        public List<SplTransferListItem> Data { get; set; } = new();
    }
}