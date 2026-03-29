namespace HotelReservation.Services;

using HotelReservation.Models;
using HotelReservation.Repositories;

public class ReservationService
{
    private readonly ReservationRepository _repository;
    private readonly ReservationDomainService _domain;

    public ReservationService()
    {
        _repository = new ReservationRepository();
        var rooms = new RoomRepository();
        _domain = new ReservationDomainService(rooms, _repository);
    }

    public string CreateReservation(string guestName, string roomId,
        DateTime checkIn, DateTime checkOut,
        int guestCount, string roomType, string email)
    {
        Console.WriteLine($"[LOG] Creating reservation for {guestName}...");

        var room = _domain.ValidateAndGetRoom(roomId, guestCount, checkIn, checkOut);
        var total = _domain.CalculatePrice(room, checkIn, checkOut);

        var reservation = new Reservation
        {
            Id       = _repository.NextId(),
            GuestName = guestName,
            RoomId   = roomId,
            CheckIn  = checkIn,
            CheckOut = checkOut,
            GuestCount = guestCount,
            RoomType = roomType,
            Status   = "Confirmed",
            Email    = email,
            TotalPrice = total
        };

        _repository.Save(reservation);

        Console.WriteLine($"[LOG] Reservation {reservation.Id} created.");
        return reservation.Id;
    }

    public Reservation? GetReservation(string id) => _repository.FindById(id);
    public List<Reservation> GetAllReservations() => _repository.GetAll();
}