namespace HotelReservation.Services;

using HotelReservation.Models;

// SRP VIOLATION (Example 2): A single method mixes multiple levels of abstraction.
// High-level business rules sit next to low-level cache manipulation and config reading.
public class CheckInService
{
    private readonly Dictionary<string, CacheEntry> _cache = new();
    private readonly Dictionary<string, Reservation> _dataStore;

    public CheckInService(Dictionary<string, Reservation> dataStore)
    {
        _dataStore = dataStore;
    }

    public void ProcessCheckIn(Reservation reservation)
    {
        // HIGH LEVEL: business rule
        if (reservation.Status != "Confirmed")
            throw new Exception($"Cannot check in: reservation is {reservation.Status}");

        // LOW LEVEL: cache manipulation
        if (_cache.ContainsKey(reservation.Id))
            _cache.Remove(reservation.Id);
        _cache[reservation.Id] = new CacheEntry(DateTime.Now, "CheckedIn");

        // HIGH LEVEL: late check-in fee logic
        var lateCheckInFee = 25m; // Hardcoded, should come from config
        if (DateTime.Now.Hour >= 22)
            reservation.TotalPrice += lateCheckInFee;

        // LOW LEVEL: direct state mutation
        reservation.Status = "CheckedIn";

        // LOW LEVEL: direct notification
        Console.WriteLine($"[SMS] Room {reservation.RoomId} is now occupied");
    }

    public void ProcessCheckOut(Reservation reservation)
    {
        if (reservation.Status != "CheckedIn")
            throw new Exception($"Cannot check out: reservation is {reservation.Status}");

        reservation.Status = "CheckedOut";

        // LOW LEVEL: cache cleanup
        if (_cache.ContainsKey(reservation.Id))
            _cache.Remove(reservation.Id);

        Console.WriteLine($"[SMS] Room {reservation.RoomId} is now free");
    }
}
