namespace HotelReservation.Interfaces;

using HotelReservation.Models;

// Linen change every day (VIP service)
public class VipCleaningPolicy : ICleaningPolicy
{
    public List<CleaningTask> GenerateTasks(Reservation reservation)
    {
        var tasks = new List<CleaningTask>();
        var current = reservation.CheckIn.AddDays(1);
        while (current < reservation.CheckOut)
        {
            tasks.Add(new CleaningTask
            {
                RoomId = reservation.RoomId,
                Date = current,
                Type = "LinenChange",
                Time = new TimeSpan(9, 0, 0)
            });
            current = current.AddDays(1);
        }
        return tasks;
    }
}
