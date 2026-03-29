namespace HotelReservation.Services.Policies;

using HotelReservation.Interfaces;
using HotelReservation.Models;

public class StrictPolicy : ICancellationPolicy
{
    public string PolicyName => "Strict";

    public decimal CalculateRefund(Reservation reservation, DateTime now)
    {
        var days = (reservation.CheckIn - now).Days;
        if (days >= 14) return reservation.TotalPrice;
        if (days >= 7) return reservation.TotalPrice * 0.5m;
        return 0m;
    }
}