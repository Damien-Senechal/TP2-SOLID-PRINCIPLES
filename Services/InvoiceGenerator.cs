namespace HotelReservation.Services;

using HotelReservation.Interfaces;
using HotelReservation.Models;

public class InvoiceGenerator
{
    public Invoice Generate(IInvoiceable reservation)
    {
        var nights = (reservation.CheckOut - reservation.CheckIn).Days;
        var pricePerNight = reservation.RoomType switch
        {
            "Standard" => 80m,
            "Suite"    => 200m,
            "Family"   => 120m,
            _          => 0m
        };
        var subtotal   = nights * pricePerNight;
        var tva        = subtotal * 0.10m;
        var touristTax = reservation.GuestCount * nights * 1.50m;

        return new Invoice
        {
            ReservationId   = reservation.Id,
            GuestName       = reservation.GuestName,
            RoomDescription = $"{reservation.RoomType} {reservation.RoomId}",
            Nights          = nights,
            Subtotal        = subtotal,
            Tva             = tva,
            TouristTax      = touristTax,
            Total           = subtotal + tva + touristTax
        };
    }

    public void PrintInvoice(Invoice invoice, IInvoiceable reservation)
    {
        Console.WriteLine($"Invoice for {invoice.GuestName}:");
        Console.WriteLine($"  Room: {invoice.RoomDescription}, " +
            $"{reservation.CheckIn:dd/MM} -> {reservation.CheckOut:dd/MM} " +
            $"({invoice.Nights} nights)");
        Console.WriteLine($"  Subtotal: {invoice.Subtotal:F2} EUR");
        Console.WriteLine($"  TVA (10%): {invoice.Tva:F2} EUR");
        Console.WriteLine($"  Tourist Tax ({reservation.GuestCount} guests x " +
            $"{invoice.Nights} nights x 1.50 EUR): {invoice.TouristTax:F2} EUR");
        Console.WriteLine($"  Total: {invoice.Total:F2} EUR");
    }
}