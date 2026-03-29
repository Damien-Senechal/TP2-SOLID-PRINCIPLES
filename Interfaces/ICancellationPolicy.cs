namespace HotelReservation.Interfaces;

using HotelReservation.Models;

public interface ICancellationPolicy
{
    string PolicyName { get; }
    decimal CalculateRefund(Reservation reservation, DateTime now);
}