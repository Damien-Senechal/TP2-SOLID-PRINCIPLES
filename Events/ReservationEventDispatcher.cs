namespace HotelReservation.Events;

// OCP GOOD EXAMPLE: Adding a new handler (e.g., audit, analytics) requires
// only registering it. The dispatcher code never changes.
public class ReservationEventDispatcher
{
    private readonly List<IReservationEventHandler> _handlers = new();

    public void Register(IReservationEventHandler handler)
    {
        _handlers.Add(handler);
    }

    public void Dispatch(ReservationCreatedEvent evt)
    {
        Console.WriteLine("[EVENT] ReservationCreated dispatched");
        foreach (var handler in _handlers)
            handler.Handle(evt);
    }
}
