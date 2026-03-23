namespace HotelReservation.Interfaces;

// ISP VIOLATION: BookingService only sends emails, HousekeepingService only
// sends SMS, yet both depend on this interface with 4 methods.
public interface INotificationService
{
    void SendEmail(string to, string subject, string body);
    void SendSms(string phoneNumber, string message);
    void SendPushNotification(string deviceId, string message);
    void SendSlackMessage(string channel, string message);
}
