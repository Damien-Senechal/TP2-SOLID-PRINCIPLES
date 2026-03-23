namespace HotelReservation.Models;

// SRP VIOLATION (Example 3): This class serves THREE actors:
// - Receptionist: lifecycle (Cancel, Status management)
// - Accountant: billing (CalculateTotal, GenerateInvoiceLine)
// - Housekeeper: cleaning schedule (GetLinenChangeDays)

public class Reservation
{
    public string Id { get; set; } = string.Empty;
    public string GuestName { get; set; } = string.Empty;
    public string RoomId { get; set; } = string.Empty;
    public DateTime CheckIn { get; set; }
    public DateTime CheckOut { get; set; }
    public int GuestCount { get; set; }
    public string RoomType { get; set; } = string.Empty;
    public string Status { get; set; } = "Confirmed"; // Confirmed, CheckedIn, CheckedOut, Cancelled
    public string CancellationPolicy { get; set; } = "Flexible";
    public string Email { get; set; } = string.Empty;
    public decimal TotalPrice { get; set; }

    // Actor: RECEPTIONIST — cancellation rules
    public void Cancel()
    {
        if (Status == "CheckedIn")
            throw new InvalidOperationException("Cannot cancel after check-in");
        Status = "Cancelled";
    }

    // Actor: ACCOUNTANT — pricing rules (TVA, tourist tax)
    public decimal CalculateTotal()
    {
        var nights = (CheckOut - CheckIn).Days;
        var pricePerNight = RoomType switch
        {
            "Standard" => 80m,
            "Suite" => 200m,
            "Family" => 120m,
            _ => 0m
        };
        var subtotal = nights * pricePerNight;
        var tva = subtotal * 0.10m;
        var touristTax = GuestCount * nights * 1.50m;
        return subtotal + tva + touristTax;
    }

    // Actor: HOUSEKEEPER — linen change schedule
    public List<DateTime> GetLinenChangeDays()
    {
        var days = new List<DateTime>();
        var current = CheckIn.AddDays(3);
        while (current < CheckOut)
        {
            days.Add(current);
            current = current.AddDays(3);
        }
        return days;
    }

    // Actor: ACCOUNTANT — invoice format
    public string GenerateInvoiceLine()
    {
        return $"{GuestName} | {CheckIn:dd/MM} -> {CheckOut:dd/MM} | {CalculateTotal():F2} EUR";
    }
}
