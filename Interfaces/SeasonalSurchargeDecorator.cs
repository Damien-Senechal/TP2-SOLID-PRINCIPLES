namespace HotelReservation.Interfaces;

using HotelReservation.Models;

// OCP GOOD EXAMPLE (Decorator pattern): Adds seasonal pricing without modifying
// the base calculator. New decorators can be stacked without touching existing code.
public class SeasonalSurchargeDecorator : IPriceCalculator
{
    private readonly IPriceCalculator _inner;
    private readonly decimal _surchargeRate;

    public SeasonalSurchargeDecorator(IPriceCalculator inner, decimal surchargeRate)
    {
        _inner = inner;
        _surchargeRate = surchargeRate;
    }

    public decimal Calculate(Reservation reservation)
    {
        var basePrice = _inner.Calculate(reservation);
        return basePrice * (1 + _surchargeRate);
    }
}
