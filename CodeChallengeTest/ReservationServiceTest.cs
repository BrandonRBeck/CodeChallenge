using CodeChallenge.Constants;
using CodeChallenge.Controllers;
using CodeChallenge.Persistence;
using CodeChallenge.Persistence.DBModel;
using CodeChallenge.RequestModel;
using CodeChallenge.ResponseModel;
using CodeChallenge.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace CodeChallengeTest;

public class ReservationServiceTest
{
    private readonly ITestOutputHelper _output;
    private readonly SchedulingContext _dbContext;
    private readonly Guid patientId = new Guid("ec1ea37e-220e-42ba-b2a3-c9cef0b77f12");
    private readonly Guid providerId = new Guid("0c80c5c9-0ce9-4aa8-bdb4-cbdc8da9be76");
    private readonly Guid appointmentId = new Guid("1a8b7434-1409-45e4-b570-06154d3a85b4");

    public ReservationServiceTest(ITestOutputHelper output)
    {
        _output = output;
        var builder = new DbContextOptionsBuilder<SchedulingContext>();
        builder.UseInMemoryDatabase(databaseName: "localDb");

        var dbContextOptions = builder.Options;
        _dbContext = new SchedulingContext(dbContextOptions);
        _dbContext.Database.EnsureDeleted();
        _dbContext.Database.EnsureCreated();
    }

    //We want to test to ensure appointments made within 24 hours are rejected
    [Fact]
    public async void TestReservationWithin24HoursRejected()
    {
        await SetupDB();
        var users = new UserService(_dbContext);
        var reservations = new ReservationService(_dbContext);
        var controller = new ReservationController(reservations, users);

        var request = new ReservationRequestModel
        {
            AppointmentId = appointmentId,
            UserId = patientId
        };
        var result = await controller.ReserveTimeSlot(request);

        Assert.Equal(400, (result.Result as ObjectResult)?.StatusCode);
    }

    //We want to test to ensure appointments are made available again if they have not been confirmed within 30 minutes
    [Fact]
    public async void TestUnconfirmedAppointmentAvailableAfter30()
    {
        await SetupDB();
        var appt = await _dbContext.Appointments.FirstOrDefaultAsync();
        appt.IsReserved = true;
        appt.ReservationHolderId = patientId;
        appt.IsConfirmed = false;
        appt.ReservationTimestamp = DateTime.UtcNow.AddMinutes(-45);
        await _dbContext.SaveChangesAsync();

        var users = new UserService(_dbContext);
        var reservations = new ReservationService(_dbContext);
        var controller = new ReservationController(reservations, users);
        
        var request = new ConfirmationRequestModel
        {
            AppointmentId = appointmentId,
            UserId = patientId
        };

        var result = await controller.ConfirmTimeslot(request);
        Assert.Equal(400, (result.Result as ObjectResult)?.StatusCode);
    }

    //We want to test to ensure appointments are not available if they were reserved but not confirmed with the last 30 minutes
    [Fact]
    public async void TestUnconfirmedAppointmentUnAvailableBefore30()
    {
        await SetupDB();
        var appt = await _dbContext.Appointments.FirstOrDefaultAsync();
        appt.IsReserved = true;
        appt.IsConfirmed = false;
        appt.ReservationHolderId = patientId;
        appt.ReservationTimestamp = DateTime.UtcNow.AddMinutes(-15);
        await _dbContext.SaveChangesAsync();
        var users = new UserService(_dbContext);
        var reservations = new ReservationService(_dbContext);
        var controller = new ReservationController(reservations, users);
        var request = new ConfirmationRequestModel
        {
            AppointmentId = appointmentId,
            UserId = patientId
        };
        var result = await controller.ConfirmTimeslot(request);
        var resultObject = GetObjectResultContent<ConfirmationResponse>(result);
        Assert.Equal(appt.AppointmentStartTime, resultObject.AppointmentTime);
        
    }

    private static T GetObjectResultContent<T>(ActionResult<T> result)
    {
        return (T)((OkObjectResult)result.Result).Value;
    }


    //Test Scaffolding
    private async Task SetupDB()
    {
        var provider = new User
        {
            UserId = providerId,
            FirstName = "Brandon",
            LastName = "Beck",
            Type = UserConstants.UserTypeProvider
        };

        var patient = new User
        {
            UserId = patientId,
            FirstName = "Daniel",
            LastName = "Harp",
            Type = UserConstants.UserTypePatient
        };

        var appointment = new Appointment
        {
            AppointmentId = appointmentId,
            AppointmentStartTime = DateTime.UtcNow.AddHours(15),
            IsConfirmed = false,
            IsReserved = false,
            ProviderId = provider.UserId
        };

        _dbContext.Users.Add(provider);
        _dbContext.Users.Add(patient);
        _dbContext.Appointments.Add(appointment);
        await _dbContext.SaveChangesAsync();
    }
}