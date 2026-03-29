namespace HotelReservation.Interfaces;

public interface ISlackNotifier
{
    void SendSlackMessage(string channel, string message);
}