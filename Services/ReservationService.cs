namespace HotelReservation.Services;

using HotelReservation.Models;
using HotelReservation.Infrastructure;

// SRP VIOLATION (Example 1): This class mixes three levels of concern:
// - INFRASTRUCTURE: direct data access, logging
// - BUSINESS: availability check, price calculation, validation
// - APPLICATION: workflow orchestration
public class ReservationService
{
    // INFRA: direct data storage
    private static readonly Dictionary<string, Reservation> _reservations = new();
    private static readonly List<Room> _rooms = new()
    {
        new Room { Id = "101", Type = "Standard", MaxGuests = 2, PricePerNight = 80m },
        new Room { Id = "102", Type = "Standard", MaxGuests = 2, PricePerNight = 80m },
        new Room { Id = "201", Type = "Suite", MaxGuests = 2, PricePerNight = 200m },
        new Room { Id = "301", Type = "Family", MaxGuests = 4, PricePerNight = 120m }
    };

    private static int _counter = 0;

    public string CreateReservation(string guestName, string roomId, DateTime checkIn,
        DateTime checkOut, int guestCount, string roomType, string email)
    {
        // INFRA: logging
        Console.WriteLine($"[LOG] Creating reservation for {guestName}...");

        // BUSINESS: find room
        var room = _rooms.FirstOrDefault(r => r.Id == roomId);
        if (room == null)
            throw new Exception($"Room {roomId} not found");

        // BUSINESS: check capacity
        if (guestCount > room.MaxGuests)
            throw new Exception($"Room {roomId} max capacity is {room.MaxGuests}");

        // BUSINESS: check availability
        var isAvailable = !_reservations.Values.Any(r =>
            r.RoomId == roomId &&
            r.Status != "Cancelled" &&
            r.CheckIn < checkOut &&
            r.CheckOut > checkIn);
        if (!isAvailable)
            throw new Exception($"Room {roomId} is not available for {checkIn:dd/MM} -> {checkOut:dd/MM}");

        // BUSINESS: calculate price
        var nights = (checkOut - checkIn).Days;
        var total = nights * room.PricePerNight;

        // APPLICATION: create and store
        _counter++;
        var reservation = new Reservation
        {
            Id = $"R-{_counter:D3}",
            GuestName = guestName,
            RoomId = roomId,
            CheckIn = checkIn,
            CheckOut = checkOut,
            GuestCount = guestCount,
            RoomType = roomType,
            Status = "Confirmed",
            Email = email,
            TotalPrice = total
        };
        _reservations[reservation.Id] = reservation;

        // INFRA: logging
        Console.WriteLine($"[LOG] Reservation {reservation.Id} created.");

        return reservation.Id;
    }

    public Reservation? GetReservation(string id)
    {
        return _reservations.TryGetValue(id, out var r) ? r : null;
    }

    public List<Reservation> GetAllReservations()
    {
        return _reservations.Values.ToList();
    }

    public static List<Room> GetRooms() => _rooms;

    public static void Reset()
    {
        _reservations.Clear();
        _counter = 0;
    }
}
