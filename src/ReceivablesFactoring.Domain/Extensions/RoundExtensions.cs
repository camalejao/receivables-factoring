
namespace ReceivablesFactoring.Domain.Extensions;

public static class RoundExtensions
{
    public static decimal Round(this decimal value, int decimals = 2)
    {
        return decimal.Round(value, decimals);
    }
}
