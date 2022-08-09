namespace Nomis.SOL.Web.Helpers;

public static class HtmlHelpers
{
    public static string Round(this decimal value, int num = 2)
    {
        return value.ToString("F"+num);
    }

    public static string Round(this double value, int num = 2)
    {
        return value.ToString("F"+num);
    }
}