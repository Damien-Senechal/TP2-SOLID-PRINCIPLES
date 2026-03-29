namespace HotelReservation.Services;

using HotelReservation.Models;

public interface IBookingRepository
{
    void Add(Reservation reservation);
}