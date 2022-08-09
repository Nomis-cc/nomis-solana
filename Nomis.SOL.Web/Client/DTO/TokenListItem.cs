using System.Globalization;
using System.Text.Json.Serialization;

namespace Nomis.SOL.Web.Client.DTO;

public class TokenListItem
{
    [JsonPropertyName("tokenAddress")]
    public string TokenAddress { get; set; }

    [JsonPropertyName("tokenAmount")]
    public TokenAmount TokenAmount { get; set; }

    [JsonPropertyName("tokenSymbol")]
    public string TokenSymbol { get; set; }
}

public class TokenAmount
{
    [JsonPropertyName("uiAmount")]
    public double UiAmount { get; set; }

    [JsonPropertyName("decimals")]
    public int Decimals { get; set; }

    public override string ToString()
    {
        return UiAmount.ToString(CultureInfo.InvariantCulture);
    }
}