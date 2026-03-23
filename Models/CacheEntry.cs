namespace HotelReservation.Models;

public class CacheEntry
{
    public DateTime Timestamp { get; set; }
    public string Status { get; set; } = string.Empty;

    public CacheEntry(DateTime timestamp, string status)
    {
        Timestamp = timestamp;
        Status = status;
    }
}
