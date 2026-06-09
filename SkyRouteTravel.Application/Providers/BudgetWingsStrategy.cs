namespace SkyRouteTravel.Application.Providers;

public class BudgetWingsStrategy : IFlightProviderStrategy
{
    public decimal CalculateFinalPrice(decimal baseFare)
    {
        // BudgetWings: Base fare - 10% promotional discount
        // Discount is always applied to the base fare only
        decimal discount = baseFare * 0.10m;
        decimal finalPrice = baseFare - discount;

        // Minimum final price is $29.99
        return Math.Max(finalPrice, 29.99m);
    }
}
