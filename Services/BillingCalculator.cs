namespace HotelReservation.Services;

using HotelReservation.Models;

public class BillingCalculator
{
    public decimal CalculateTotal(Reservation r)
    {
        var nights = (r.CheckOut - r.CheckIn).Days;
        var pricePerNight = r.RoomType switch
        {
            "Standard" => 80m,
            "Suite"    => 200m,
            "Family"   => 120m,
            _          => 0m
        };
        var subtotal   = nights * pricePerNight;
        var tva        = subtotal * 0.10m;
        var touristTax = r.GuestCount * nights * 1.50m;
        return subtotal + tva + touristTax;
    }

    public string GenerateInvoiceLine(Reservation r)
    {
        var total = CalculateTotal(r);
        return $"{r.GuestName} | {r.CheckIn:dd/MM} -> {r.CheckOut:dd/MM} | {total:F2} EUR";
    }
}