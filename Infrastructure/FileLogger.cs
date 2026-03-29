namespace HotelReservation.Infrastructure;

using HotelReservation.Services;

public class FileLogger : IBookingLogger
{
    public void Log(string message) =>
        Console.WriteLine($"[LOG] {message}");
}