namespace SkyRouteTravel.Application.Providers;

public interface IFlightProviderStrategyFactory
{
    IFlightProviderStrategy GetStrategy(string providerName);
}

public class FlightProviderStrategyFactory : IFlightProviderStrategyFactory
{
    public IFlightProviderStrategy GetStrategy(string providerName)
    {
        if (string.IsNullOrWhiteSpace(providerName))
        {
            return new DefaultProviderStrategy();
        }

        return providerName.ToLowerInvariant() switch
        {
            "globalair" => new GlobalAirStrategy(),
            "budgetwings" => new BudgetWingsStrategy(),
            _ => new DefaultProviderStrategy()
        };
    }
}

public class DefaultProviderStrategy : IFlightProviderStrategy
{
    public decimal CalculateFinalPrice(decimal baseFare)
    {
        return baseFare; // Default: no changes
    }
}
