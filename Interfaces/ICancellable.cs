namespace HotelReservation.Interfaces;

// LSP VIOLATION (Example 1): NonRefundableReservation implements this interface
// but throws on Cancel(), breaking the substitution principle.
public interface ICancellable
{
    string Id { get; }
    string GuestName { get; }
    string Status { get; }
    decimal TotalPrice { get; }
    void Cancel();
    decimal CalculateRefund();
}
