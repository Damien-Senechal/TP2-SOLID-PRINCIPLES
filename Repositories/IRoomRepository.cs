namespace HotelReservation.Repositories;

using HotelReservation.Models;

public interface IRoomRepository
{
    Room? GetById(string roomId);
    List<Room> GetAvailableRooms(DateTime from, DateTime to);
    void Save(Room room);
}
