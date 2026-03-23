namespace HotelReservation.Models;

public class Invoice
{
    public string ReservationId { get; set; } = string.Empty;
    public string GuestName { get; set; } = string.Empty;
    public string RoomDescription { get; set; } = string.Empty;
    public int Nights { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Tva { get; set; }
    public decimal TouristTax { get; set; }
    public decimal Total { get; set; }
}
