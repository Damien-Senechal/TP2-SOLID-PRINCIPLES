namespace HotelReservation.Interfaces;

using HotelReservation.Models;

public class BasePriceCalculator : IPriceCalculator
{
    public decimal Calculate(Reservation reservation)
    {
        var nights = (reservation.CheckOut - reservation.CheckIn).Days;
        return nights * GetBasePrice(reservation.RoomType);
    }

    private decimal GetBasePrice(string type) => type switch
    {
        "Standard" => 80m,
        "Suite" => 200m,
        "Family" => 120m,
        _ => 0m
    };
}
