namespace HotelReservation.Interfaces;

using HotelReservation.Models;

// OCP GOOD EXAMPLE: Adding a new pricing rule (e.g., loyalty discount) only
// requires creating a new decorator class. No existing code needs to change.
public interface IPriceCalculator
{
    decimal Calculate(Reservation reservation);
}
