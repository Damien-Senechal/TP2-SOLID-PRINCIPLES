namespace HotelReservation.Models;

public class CleaningTask
{
    public string RoomId { get; set; } = string.Empty;
    public DateTime Date { get; set; }
    public string Type { get; set; } = string.Empty; // "LinenChange", "FullCleaning", "InitialSetup"
    public string HousekeeperEmail { get; set; } = string.Empty;
    public TimeSpan Time { get; set; }
}
