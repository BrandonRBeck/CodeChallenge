using CodeChallenge.Models;

namespace CodeChallenge.Services;

public interface IReservationService
{
    Task<ReservationResponse> ReverseString(string userInput);
}
