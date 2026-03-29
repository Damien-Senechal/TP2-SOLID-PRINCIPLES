namespace HotelReservation.Services.Policies;

using HotelReservation.Interfaces;
using HotelReservation.Models;

public class FlexiblePolicy : ICancellationPolicy
{
    public string PolicyName => "Flexible";

    public decimal CalculateRefund(Reservation reservation, DateTime now)
    {
        var days = (reservation.CheckIn - now).Days;
        return days >= 1 ? reservation.TotalPrice : 0m;
    }
}