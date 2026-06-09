using FluentAssertions;
using SkyRouteTravel.Application.Providers;

namespace SkyRouteTravel.Tests.Providers;

public class BudgetWingsStrategyTests
{
    private readonly BudgetWingsStrategy _sut = new();

    [Theory]
    [InlineData(100.00, 90.00)]
    [InlineData(200.00, 180.00)]
    [InlineData(500.00, 450.00)]
    public void CalculateFinalPrice_AppliesTenPercentDiscount(decimal baseFare, decimal expectedPrice)
    {
        var result = _sut.CalculateFinalPrice(baseFare);

        result.Should().Be(expectedPrice);
    }

    [Fact]
    public void CalculateFinalPrice_WhenDiscountedPriceBelowMinimum_ReturnsMinimumPrice()
    {
        // 20.00 * 0.90 = 18.00 → below minimum of 29.99
        var result = _sut.CalculateFinalPrice(20.00m);

        result.Should().Be(29.99m);
    }

    [Fact]
    public void CalculateFinalPrice_WhenDiscountedPriceEqualsMinimum_ReturnsMinimumPrice()
    {
        // 33.32 * 0.90 ≈ 29.988 → just below 29.99
        var result = _sut.CalculateFinalPrice(29.99m);

        result.Should().Be(29.99m);
    }

    [Fact]
    public void CalculateFinalPrice_WhenBaseFareIsZero_ReturnsMinimumPrice()
    {
        var result = _sut.CalculateFinalPrice(0m);

        result.Should().Be(29.99m);
    }

    [Fact]
    public void CalculateFinalPrice_DiscountAppliedOnlyToBaseFare()
    {
        // Verifies the discount is applied only to baseFare, not to minimum
        var result = _sut.CalculateFinalPrice(100m);

        result.Should().Be(90m);
        result.Should().NotBe(100m - (29.99m * 0.10m));
    }
}
