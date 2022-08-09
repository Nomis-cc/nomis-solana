namespace Nomis.SOL.Web.Services;

using Swashbuckle.AspNetCore.Annotations;

public class WalletScoreResult
{
    [SwaggerSchema("Nomis Score in range of [0; 1]", ReadOnly = true)]
    public double Score { get; set; }

    [SwaggerSchema("Additional stat data? used in score calculations.", ReadOnly = true)]
    public WalletStats Stats { get; set; }
}