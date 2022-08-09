#nullable enable
namespace Nomis.SOL.Web.Client;

public class NoDataException : Exception
{
    public NoDataException(string? message) : base(message)
    {
    }
}