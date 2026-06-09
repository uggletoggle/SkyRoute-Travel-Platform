using FluentAssertions;
using SkyRouteTravel.Application.Providers;

namespace SkyRouteTravel.Tests.Providers;

public class GlobalAirStrategyTests
{
    private readonly GlobalAirStrategy _sut = new();

    [Theory]
    [InlineData(100.00, 115.00)]
    [InlineData(200.00, 230.00)]
    [InlineData(0.00, 0.00)]
    public void CalculateFinalPrice_AppliesFifteenPercentSurcharge(decimal baseFare, decimal expectedPrice)
    {
        var result = _sut.CalculateFinalPrice(baseFare);

        result.Should().Be(expectedPrice);
    }

    [Fact]
    public void CalculateFinalPrice_RoundsToTwoDecimalPlaces()
    {
        // 100.10 * 1.15 = 115.115 → rounds to 115.12
        var result = _sut.CalculateFinalPrice(100.10m);

        result.Should().Be(115.12m);
    }

    [Fact]
    public void CalculateFinalPrice_LargeFare_RoundsCorrectly()
    {
        // 999.99 * 1.15 = 1149.9885 → rounds to 1149.99
        var result = _sut.CalculateFinalPrice(999.99m);

        result.Should().Be(1149.99m);
    }
}
