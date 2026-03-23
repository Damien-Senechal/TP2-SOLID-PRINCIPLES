namespace HotelReservation.Interfaces;

using HotelReservation.Models;

// OCP GOOD EXAMPLE (Strategy pattern): Adding a new cleaning policy only
// requires creating a new implementation. No existing code needs to change.
public interface ICleaningPolicy
{
    List<CleaningTask> GenerateTasks(Reservation reservation);
}
