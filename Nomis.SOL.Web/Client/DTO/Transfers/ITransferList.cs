using System.Text.Json.Serialization;

namespace Nomis.SOL.Web.Client.DTO.Transfers
{
    public interface ITransferList<TListItem> where TListItem : ITransferListItem
    {
        [JsonPropertyName("data")]
        public List<TListItem> Data { get; set; }
    }
}