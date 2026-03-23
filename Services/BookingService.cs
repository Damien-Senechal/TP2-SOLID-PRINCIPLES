namespace HotelReservation.Services;

using HotelReservation.Infrastructure;
using HotelReservation.Models;

// DIP VIOLATION (Example 1): High-level business module directly depends on
// low-level infrastructure modules (InMemoryReservationStore, FileLogger).
// Impossible to change storage or logging without modifying this class.
public class BookingService
{
    // Direct dependency on concrete implementations
    private readonly InMemoryReservationStore _store = new();
    private readonly FileLogger _logger = new();

    private int _counter = 0;

    public string CreateReservation(string guestName, string roomId, DateTime checkIn,
        DateTime checkOut, int guestCount, string roomType, string email)
    {
        _logger.Log($"Creating reservation for {guestName}...");

        var nights = (checkOut - checkIn).Days;
        var pricePerNight = roomType switch
        {
            "Standard" => 80m,
            "Suite" => 200m,
            "Family" => 120m,
            _ => throw new Exception($"Unknown room type: {roomType}")
        };

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
            TotalPrice = nights * pricePerNight
        };

        _store.Add(reservation);
        _logger.Log($"Reservation {reservation.Id} created.");

        return reservation.Id;
    }
}
