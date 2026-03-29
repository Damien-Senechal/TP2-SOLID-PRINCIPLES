namespace HotelReservation.Services.Policies;

using HotelReservation.Interfaces;
using HotelReservation.Models;

public class ModeratePolicy : ICancellationPolicy
{
    public string PolicyName => "Moderate";

    public decimal CalculateRefund(Reservation reservation, DateTime now)
    {
        var days = (reservation.CheckIn - now).Days;
        if (days >= 5) return reservation.TotalPrice;
        if (days >= 2) return reservation.TotalPrice * 0.5m;
        return 0m;
    }
}