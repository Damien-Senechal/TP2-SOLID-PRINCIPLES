namespace HotelReservation.Interfaces;

// LSP VIOLATION: Cancel() throws instead of performing the expected behavior.
// Code that calls ICancellable.Cancel() will crash when given a NonRefundableReservation.
public class NonRefundableReservation : ICancellable
{
    public string Id { get; set; } = string.Empty;
    public string GuestName { get; set; } = string.Empty;
    public string Status { get; set; } = "Confirmed";
    public decimal TotalPrice { get; set; }

    public void Cancel()
    {
        throw new InvalidOperationException(
            "Non-refundable reservations cannot be cancelled");
    }

    public decimal CalculateRefund()
    {
        return 0m;
    }
}
