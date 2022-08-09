using System.Text.Json.Serialization;
using Nomis.SOL.Web.Helpers;

namespace Nomis.SOL.Web.Client.DTO
{
    public class MagicEdenWalletActivity
    {
        [JsonPropertyName("signature")]
        public string Signature { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("source")]
        public string Source { get; set; }

        [JsonPropertyName("tokenMint")]
        public string TokenMint { get; set; }

        [JsonPropertyName("collection")]
        public string Collection { get; set; }

        [JsonPropertyName("collectionSymbol")]
        public string CollectionSymbol { get; set; }

        [JsonPropertyName("slot")]
        public long Slot { get; set; }

        [JsonPropertyName("blockTime")]
        public long BlockTime { get; set; }

        public DateTime Date => BlockTime.ToDateTime();

        [JsonPropertyName("buyer")]
        public string Buyer { get; set; }

        [JsonPropertyName("buyerReferral")]
        public string BuyerReferral { get; set; }

        [JsonPropertyName("seller")]
        public string Seller { get; set; }

        [JsonPropertyName("sellerReferral")]
        public string SellerReferral { get; set; }

        [JsonPropertyName("price")]
        public decimal Price { get; set; }
    }
}