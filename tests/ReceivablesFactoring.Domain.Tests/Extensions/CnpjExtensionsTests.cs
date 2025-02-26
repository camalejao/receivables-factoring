using ReceivablesFactoring.Domain.Extensions;
using Xunit;

namespace ReceivablesFactoring.Domain.Tests.Extensions;

public class CnpjExtensionsTests
{
    [Theory]
    [InlineData("12345678000195", true)]
    [InlineData("12345678000196", false)]
    [InlineData("12345678", false)]
    [InlineData("", false)]
    [InlineData(null, false)]
    public void IsValidCnpj_ShouldValidateCnpjCorrectly(string cnpj, bool expected)
    {
        var result = cnpj.IsValidCnpj();
        Assert.Equal(expected, result);
    }
}
