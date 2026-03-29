namespace HotelReservation.Services;

using HotelReservation.Models;

public class HousekeepingScheduler
{
    public List<DateTime> GetLinenChangeDays(Reservation r)
    {
        var days    = new List<DateTime>();
        var current = r.CheckIn.AddDays(3);
        while (current < r.CheckOut)
        {
            days.Add(current);
            current = current.AddDays(3);
        }
        return days;
    }
}