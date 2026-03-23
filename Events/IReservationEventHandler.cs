namespace HotelReservation.Events;

// OCP GOOD EXAMPLE (Observer pattern): New handlers can be registered
// without modifying the dispatcher or existing handlers.
public interface IReservationEventHandler
{
    void Handle(ReservationCreatedEvent evt);
}
