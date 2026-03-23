namespace HotelReservation.Infrastructure;

public class EmailSender
{
    public void Send(string to, string subject, string body)
    {
        Console.WriteLine($"[EMAIL] {subject} sent to {to}");
    }
}
