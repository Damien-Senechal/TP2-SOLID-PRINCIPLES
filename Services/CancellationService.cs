namespace HotelReservation.Services;

using HotelReservation.Models;

// OCP VIOLATION: Adding a new cancellation policy (e.g., "SuperFlexible")
// requires opening this class and adding a new case to the switch.
public class CancellationService
{
    public decimal CalculateRefund(Reservation reservation, DateTime now)
    {
        var daysBeforeCheckIn = (reservation.CheckIn - now).Days;

        switch (reservation.CancellationPolicy)
        {
            case "Flexible":
                return daysBeforeCheckIn >= 1 ? reservation.TotalPrice : 0m;

            case "Moderate":
                if (daysBeforeCheckIn >= 5) return reservation.TotalPrice;
                if (daysBeforeCheckIn >= 2) return reservation.TotalPrice * 0.5m;
                return 0m;

            case "Strict":
                if (daysBeforeCheckIn >= 14) return reservation.TotalPrice;
                if (daysBeforeCheckIn >= 7) return reservation.TotalPrice * 0.5m;
                return 0m;

            case "NonRefundable":
                return 0m;

            default:
                throw new ArgumentException(
                    $"Unknown cancellation policy: {reservation.CancellationPolicy}");
        }
    }

    public void CancelReservation(Reservation reservation, DateTime now)
    {
        var refund = CalculateRefund(reservation, now);
        reservation.Cancel();
        Console.WriteLine(
            $"[OK] Reservation {reservation.Id} cancelled " +
            $"({reservation.CancellationPolicy} policy: " +
            $"{(refund == reservation.TotalPrice ? "full" : "partial")} refund of {refund:F2} EUR)");
    }
}
