
using CodeChallenge.Persistence.DBModel;
using CodeChallenge.RequestModel;
using CodeChallenge.ResponseModel;

namespace CodeChallenge.Services;

public interface IReservationService
{
    Task<List<Appointment>> GetProviderAvailabilty(User provider);
    Task<ConfirmationResponse> ConfirmTimeslot(ConfirmationRequestModel confirmRequest);
    Task<ReservationResponse> ReserveTimeSlot(ReservationRequestModel reservationRequest);
    Task ScheduleProviderAvailability(User provider, ProviderAvailabilityModel availability);
}
