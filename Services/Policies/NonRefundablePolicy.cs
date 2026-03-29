namespace HotelReservation.Services.Policies;

using HotelReservation.Interfaces;
using HotelReservation.Models;

public class NonRefundablePolicy : ICancellationPolicy
{
    public string PolicyName => "NonRefundable";

    public decimal CalculateRefund(Reservation reservation, DateTime now) => 0m;
}