using CodeChallenge.Persistence;
using CodeChallenge.Persistence.DBModel;
using CodeChallenge.RequestModel;
using CodeChallenge.ResponseModel;
using Microsoft.EntityFrameworkCore;

namespace CodeChallenge.Services;

public class ReservationService : IReservationService
{
    public SchedulingContext _dbContext;

    public ReservationService(
        SchedulingContext schedulingContext
        )
    {
        _dbContext = schedulingContext;
    }

    public async Task<List<Appointment>> GetProviderAvailabilty(User provider)
    {
        var appts =  await _dbContext.Appointments.Where(
        x => !x.IsConfirmed && 
        x.ProviderId == provider.UserId &&
        (!x.IsReserved || x.IsReserved && x.ReservationTimestamp < DateTime.UtcNow.AddMinutes(-30)) &&
        x.AppointmentStartTime > DateTime.UtcNow.AddHours(24)
        ).ToListAsync();

        return appts;
    }

    public async Task<ConfirmationResponse> ConfirmTimeslot(ConfirmationRequestModel confirmRequest) 
    {
        var appt = await _dbContext.Appointments.FirstOrDefaultAsync(x => x.AppointmentId == confirmRequest.AppointmentId);
        if (appt == null)
        {
            throw new BadHttpRequestException("The provided AppointmentId was not found");
        }

        if (!appt.IsReserved)
        {
            throw new BadHttpRequestException("This appointment has not been reserved and cannot be confirmed.");
        }
        if(appt.ReservationTimestamp <= DateTime.UtcNow.AddMinutes(-30))
        {
            throw new BadHttpRequestException("Appointments must be confirmed within 30 minutes. Please try reserving a new appointment and confirm within 30 minutes.");
        }
        if (appt.ReservationHolderId != confirmRequest.UserId)
        {
            throw new BadHttpRequestException("This appointment was reserved by another user and cannot be confirmed.");
        }
        appt.IsConfirmed = true;
        await _dbContext.SaveChangesAsync();

        return new ConfirmationResponse
        {
            AppointmentTime = appt.AppointmentStartTime,
            ProviderId = appt.ProviderId,
            ResponseMessage = "Your appointment has successfully been confirmed. Thank You"
        };
}
    public async Task<ReservationResponse> ReserveTimeSlot(ReservationRequestModel reservationRequest) 
    {
        var appt = await _dbContext.Appointments.FirstOrDefaultAsync(x => x.AppointmentId == reservationRequest.AppointmentId);
        if(appt == null)
        {
            throw new BadHttpRequestException("The provided AppointmentId was not found");
        }

        if (appt.IsReserved)
        {
            throw new BadHttpRequestException("This appointment slot has already been taken. Please choose a new appointment.");
        }
        if(appt.AppointmentStartTime <= DateTime.UtcNow.AddHours(24))
        {
            throw new BadHttpRequestException("Appointments cannot be made with less than 24 hour notice.");
        }
        appt.IsReserved = true;
        appt.ReservationTimestamp = DateTime.UtcNow;
        appt.ReservationHolderId = reservationRequest.UserId;
        await _dbContext.SaveChangesAsync();

        return new ReservationResponse
        {
            AppointmentScheduled = true,
            AppointmentTime = appt.AppointmentStartTime,
            ProviderId = appt.ProviderId,
            ResponseMessage = "Your appointment has successfully been reserved. Please make sure you confirm within 30 minutes"
        };
    }
    public async Task ScheduleProviderAvailability(User provider, ProviderAvailabilityModel availability) 
    {
        var appts = new List<Appointment>();

        var starttime = availability.AvailabilityStartTime;
        //Should do: Ensure startime and endtime are on the same day.
        //Could also have hardcoded weekend, after hours times and skip those times.
        //Should also slide 15 minute blocks into nearest timeslot instead of assuming provider submits on the hour
        while(starttime.AddMinutes(15) < availability.AvailabilityEndTime)
        {
            appts.Add(new Appointment
            {
                ProviderId = provider.UserId,
                AppointmentStartTime = starttime,
                IsReserved = false,
                IsConfirmed = false,
                ReservationHolderId = null,
                ReservationTimestamp = null
            });
            starttime = starttime.AddMinutes(15);
        }

        await _dbContext.Appointments.AddRangeAsync(appts);
        await _dbContext.SaveChangesAsync();
    }
}
