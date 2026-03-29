namespace HotelReservation.Services;

using HotelReservation.Models;

public class CheckInService
{
    private readonly Dictionary<string, CacheEntry> _cache = new();
    private readonly Dictionary<string, Reservation> _dataStore;

    public CheckInService(Dictionary<string, Reservation> dataStore)
    {
        _dataStore = dataStore;
    }
    
    public void ProcessCheckIn(Reservation reservation)
    {
        ValidateCheckIn(reservation);
        ApplyLateCheckInFeeIfNeeded(reservation);
        UpdateStatus(reservation, "CheckedIn");
        NotifyRoomOccupied(reservation.RoomId);
    }

    public void ProcessCheckOut(Reservation reservation)
    {
        ValidateCheckOut(reservation);
        UpdateStatus(reservation, "CheckedOut");
        NotifyRoomFree(reservation.RoomId);
    }

    private void ValidateCheckIn(Reservation reservation)
    {
        if (reservation.Status != "Confirmed")
            throw new Exception($"Cannot check in: reservation is {reservation.Status}");
    }

    private void ValidateCheckOut(Reservation reservation)
    {
        if (reservation.Status != "CheckedIn")
            throw new Exception($"Cannot check out: reservation is {reservation.Status}");
    }

    private void ApplyLateCheckInFeeIfNeeded(Reservation reservation)
    {
        const decimal lateCheckInFee = 25m;
        if (DateTime.Now.Hour >= 22)
            reservation.TotalPrice += lateCheckInFee;
    }

    private void UpdateStatus(Reservation reservation, string newStatus)
    {
        RefreshCache(reservation.Id, newStatus);
        reservation.Status = newStatus;
    }

    private void RefreshCache(string reservationId, string status)
    {
        _cache.Remove(reservationId);
        _cache[reservationId] = new CacheEntry(DateTime.Now, status);
    }

    private void NotifyRoomOccupied(string roomId) =>
        Console.WriteLine($"[SMS] Room {roomId} is now occupied");

    private void NotifyRoomFree(string roomId) =>
        Console.WriteLine($"[SMS] Room {roomId} is now free");
}