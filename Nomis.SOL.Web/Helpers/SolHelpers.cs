namespace Nomis.SOL.Web.Helpers;

public static class SolHelpers
{
    public static decimal ToSol(this long valueInLamports)
    {
        return valueInLamports * (decimal) 0.000000001;
    }

    public static DateTime ToDateTime(this long unixTimeStamp)
    {
        DateTime dateTime = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dateTime = dateTime.AddSeconds(unixTimeStamp).ToLocalTime();
        return dateTime;
    }
}