namespace SkyRouteTravel.Application.Providers;

public interface IFlightProviderStrategy
{
    decimal CalculateFinalPrice(decimal baseFare);
}
