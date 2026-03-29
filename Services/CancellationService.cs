namespace HotelReservation.Services;

using HotelReservation.Interfaces;
using HotelReservation.Models;
using HotelReservation.Services.Policies;

public class CancellationService
{
    private readonly List<ICancellationPolicy> _policies = new()
    {
        new FlexiblePolicy(),
        new ModeratePolicy(),
        new StrictPolicy(),
        new NonRefundablePolicy()
    };

    private ICancellationPolicy GetPolicy(string policyName) =>
        _policies.FirstOrDefault(p => p.PolicyName == policyName)
            ?? throw new ArgumentException($"Unknown cancellation policy: {policyName}");

    public decimal CalculateRefund(Reservation reservation, DateTime now)
    {
        var policy = GetPolicy(reservation.CancellationPolicy);
        return policy.CalculateRefund(reservation, now);
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