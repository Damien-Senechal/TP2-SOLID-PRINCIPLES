namespace HotelReservation.Repositories;

using HotelReservation.Models;

public interface IReservationWriter
{
    void Add(Reservation reservation);
    void Update(Reservation reservation);
    void Delete(string id);
}