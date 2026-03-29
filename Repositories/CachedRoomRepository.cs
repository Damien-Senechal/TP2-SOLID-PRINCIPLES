namespace HotelReservation.Repositories;

using HotelReservation.Models;

public class CachedRoomRepository : IRoomRepository
{
    private readonly IRoomRepository _inner;
    private readonly Dictionary<string, Room> _cache = new();

    public CachedRoomRepository(IRoomRepository inner)
    {
        _inner = inner;
    }

    public Room? GetById(string roomId)
    {
        if (!_cache.ContainsKey(roomId))
        {
            var room = _inner.GetById(roomId);
            if (room != null)
                _cache[roomId] = room;
            return room;
        }
        return _cache[roomId];
    }

    public List<Room> GetAvailableRooms(DateTime from, DateTime to)
    {
        return _inner.GetAvailableRooms(from, to);
    }

    public void Save(Room room)
    {
        _inner.Save(room);
        _cache.Remove(room.Id);
    }
}
