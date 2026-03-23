namespace HotelReservation.Events;

public class HousekeepingSetupHandler : IReservationEventHandler
{
    public void Handle(ReservationCreatedEvent evt)
    {
        Console.WriteLine($"[HOUSEKEEPING] Room {evt.RoomId} scheduled for initial cleaning");
    }
}
