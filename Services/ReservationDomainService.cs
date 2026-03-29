namespace HotelReservation.Services;

using HotelReservation.Infrastructure;
using HotelReservation.Models;
using HotelReservation.Repositories;

public class ReservationDomainService
{
    private readonly RoomRepository _rooms;
    private readonly ReservationRepository _reservations;

    public ReservationDomainService(RoomRepository rooms, ReservationRepository reservations)
    {
        _rooms = rooms;
        _reservations = reservations;
    }

    public Room ValidateAndGetRoom(string roomId, int guestCount,
                                   DateTime checkIn, DateTime checkOut)
    {
        var room = _rooms.FindById(roomId)
            ?? throw new Exception($"Room {roomId} not found");

        if (guestCount > room.MaxGuests)
            throw new Exception($"Room {roomId} max capacity is {room.MaxGuests}");

        if (!_reservations.IsRoomAvailable(roomId, checkIn, checkOut))
            throw new Exception(
                $"Room {roomId} is not available for {checkIn:dd/MM} -> {checkOut:dd/MM}");

        return room;
    }

    public decimal CalculatePrice(Room room, DateTime checkIn, DateTime checkOut)
    {
        var nights = (checkOut - checkIn).Days;
        return nights * room.PricePerNight;
    }
}