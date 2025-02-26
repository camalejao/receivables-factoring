using ReceivablesFactoring.Domain.Extensions;
using Xunit;

namespace ReceivablesFactoring.Domain.Tests.Extensions;

public class RoundExtensionsTests
{
    [Theory]
    [InlineData(123.4567, 2, 123.46)]
    [InlineData(123.454, 2, 123.45)]
    [InlineData(123.4567, 0, 123)]
    public void Round_ShouldRoundCorrectly(decimal value, int decimals, decimal expected)
    {
        var result = value.Round(decimals);
        Assert.Equal(expected, result);
    }
}
