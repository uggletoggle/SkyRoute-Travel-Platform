using FluentAssertions;
using NSubstitute;
using SkyRouteTravel.Application.Providers;

namespace SkyRouteTravel.Tests.Providers;

public class ProviderServiceTests
{
    private readonly IProviderRepository _providerRepository;
    private readonly ProviderService _sut;

    public ProviderServiceTests()
    {
        _providerRepository = Substitute.For<IProviderRepository>();
        _sut = new ProviderService(_providerRepository);
    }

    [Fact]
    public async Task GetProvidersAsync_DelegatesToRepository()
    {
        var expected = new List<string> { "GlobalAir", "BudgetWings" };
        _providerRepository.GetProvidersAsync().Returns(expected);

        var result = await _sut.GetProvidersAsync();

        result.Should().BeEquivalentTo(expected);
    }

    [Fact]
    public async Task GetProvidersAsync_WhenNoProviders_ReturnsEmptyCollection()
    {
        _providerRepository.GetProvidersAsync().Returns(Enumerable.Empty<string>());

        var result = await _sut.GetProvidersAsync();

        result.Should().BeEmpty();
    }
}
