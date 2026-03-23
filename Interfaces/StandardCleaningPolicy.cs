namespace HotelReservation.Interfaces;

using HotelReservation.Models;

// Linen change every 3 days
public class StandardCleaningPolicy : ICleaningPolicy
{
    public List<CleaningTask> GenerateTasks(Reservation reservation)
    {
        var tasks = new List<CleaningTask>();
        var current = reservation.CheckIn.AddDays(3);
        while (current < reservation.CheckOut)
        {
            tasks.Add(new CleaningTask
            {
                RoomId = reservation.RoomId,
                Date = current,
                Type = "LinenChange",
                Time = new TimeSpan(10, 0, 0)
            });
            current = current.AddDays(3);
        }
        return tasks;
    }
}
