namespace HotelReservation.Interfaces;

public interface IPushNotifier
{
    void SendPushNotification(string deviceId, string message);
}