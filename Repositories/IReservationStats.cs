namespace HotelReservation.Repositories;

public interface IReservationStats
{
    decimal GetTotalRevenue(DateTime from, DateTime to);
    Dictionary<string, int> GetOccupancyStats(DateTime from, DateTime to);
}