#nullable enable
namespace Nomis.SOL.Web.Client;

public class InvalidAddressException : Exception
{
    public InvalidAddressException(string? message) : base(message)
    {
    }
}