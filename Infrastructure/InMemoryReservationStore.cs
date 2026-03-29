namespace HotelReservation.Infrastructure;

using HotelReservation.Models;
using HotelReservation.Services;

public class InMemoryReservationStore : IBookingRepository
{
    private readonly List<Reservation> _store = new();

    public void Add(Reservation reservation) => _store.Add(reservation);
}