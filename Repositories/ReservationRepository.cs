namespace HotelReservation.Repositories;

using HotelReservation.Models;

public class ReservationRepository
{
    private static readonly Dictionary<string, Reservation> _store = new();
    private static int _counter = 0;

    public void Save(Reservation reservation) => _store[reservation.Id] = reservation;

    public Reservation? FindById(string id) =>
        _store.TryGetValue(id, out var r) ? r : null;

    public List<Reservation> GetAll() => _store.Values.ToList();

    public bool IsRoomAvailable(string roomId, DateTime checkIn, DateTime checkOut) =>
        !_store.Values.Any(r =>
            r.RoomId == roomId &&
            r.Status != "Cancelled" &&
            r.CheckIn < checkOut &&
            r.CheckOut > checkIn);

    public string NextId() => $"R-{++_counter:D3}";

    public static void Reset()
    {
        _store.Clear();
        _counter = 0;
    }
}