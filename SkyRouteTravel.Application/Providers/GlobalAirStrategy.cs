namespace SkyRouteTravel.Application.Providers;

public class GlobalAirStrategy : IFlightProviderStrategy
{
    public decimal CalculateFinalPrice(decimal baseFare)
    {
        // GlobalAir: Base fare + 15% fuel surcharge
        decimal finalPrice = baseFare * 1.15m;
        return Math.Round(finalPrice, 2);
    }
}
