namespace HotelReservation.Models;

public class Room
{
    public string Id { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // "Standard", "Suite", "Family"
    public int MaxGuests { get; set; }
    public decimal PricePerNight { get; set; }
    public bool IsAvailable { get; set; } = true;
}
