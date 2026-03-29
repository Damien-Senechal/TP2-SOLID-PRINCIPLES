namespace HotelReservation.Repositories;

using HotelReservation.Models;

public class RoomRepository
{
    private static readonly List<Room> _rooms = new()
    {
        new Room { Id = "101", Type = "Standard", MaxGuests = 2, PricePerNight = 80m },
        new Room { Id = "102", Type = "Standard", MaxGuests = 2, PricePerNight = 80m },
        new Room { Id = "201", Type = "Suite",    MaxGuests = 2, PricePerNight = 200m },
        new Room { Id = "301", Type = "Family",   MaxGuests = 4, PricePerNight = 120m }
    };

    public Room? FindById(string id) => _rooms.FirstOrDefault(r => r.Id == id);
    public List<Room> GetAll() => _rooms;
}