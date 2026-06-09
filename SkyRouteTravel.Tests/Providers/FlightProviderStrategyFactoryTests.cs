using FluentAssertions;
using SkyRouteTravel.Application.Providers;

namespace SkyRouteTravel.Tests.Providers;

public class FlightProviderStrategyFactoryTests
{
    private readonly FlightProviderStrategyFactory _sut = new();

    [Theory]
    [InlineData("GlobalAir")]
    [InlineData("globalair")]
    [InlineData("GLOBALAIR")]
    public void GetStrategy_GlobalAirVariants_ReturnsGlobalAirStrategy(string providerName)
    {
        var strategy = _sut.GetStrategy(providerName);

        strategy.Should().BeOfType<GlobalAirStrategy>();
    }

    [Theory]
    [InlineData("BudgetWings")]
    [InlineData("budgetwings")]
    [InlineData("BUDGETWINGS")]
    public void GetStrategy_BudgetWingsVariants_ReturnsBudgetWingsStrategy(string providerName)
    {
        var strategy = _sut.GetStrategy(providerName);

        strategy.Should().BeOfType<BudgetWingsStrategy>();
    }

    [Theory]
    [InlineData("UnknownAir")]
    [InlineData("SomeOtherProvider")]
    public void GetStrategy_UnknownProvider_ReturnsDefaultStrategy(string providerName)
    {
        var strategy = _sut.GetStrategy(providerName);

        strategy.Should().BeOfType<DefaultProviderStrategy>();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void GetStrategy_NullOrWhitespace_ReturnsDefaultStrategy(string providerName)
    {
        var strategy = _sut.GetStrategy(providerName);

        strategy.Should().BeOfType<DefaultProviderStrategy>();
    }
}
