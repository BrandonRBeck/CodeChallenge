using CodeChallenge.Controllers;
using CodeChallenge.Models;
using CodeChallenge.Persistence;
using CodeChallenge.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Abstractions;

namespace CodeChallengeTest;

public class ReservationServiceTest
{
    private readonly ITestOutputHelper _output;
    private readonly SchedulingContext dbContext;

    public ReservationServiceTest(ITestOutputHelper output)
    {
        _output = output;
        var builder = new DbContextOptionsBuilder<SchedulingContext>();
        builder.UseInMemoryDatabase(databaseName: "localDb");

        var dbContextOptions = builder.Options;
        dbContext = new SchedulingContext(dbContextOptions);
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();
    }

    //We want to test to ensure appointments made within 24 hours are rejected
    [Fact]
    public async void TestReservationWithin24HoursRejected()
    {
        var reservations = new ReservationService();
        var controller = new ReservationController(reservations);
        var result = await controller.ReserveTimeSlot();
        
        var resultObject = GetObjectResultContent<ReservationResponse>(result);
        Assert.IsAssignableFrom<BadRequestObjectResult>(resultObject);
    }

    //We want to test to ensure appointments are made available again if they have not been confirmed within 30 minutes
    [Fact]
    public async void TestUnconfirmedAppointmentAvailableAfter30()
    {
        var reservations = new ReservationService();
        var controller = new ReservationController(reservations);
        var result = await controller.ReserveTimeSlot();
        var resultObject = GetObjectResultContent<ReservationResponse>(result);
        Assert.True(resultObject.AppointmentScheduled);
    }

    //We want to test to ensure appointments are not available if they were reserved but not confirmed with the last 30 minutes
    [Fact]
    public async void TestUnconfirmedAppointmentUnAvailableBefore30()
    {
        var reservations = new ReservationService();
        var controller = new ReservationController(reservations);
        var result = await controller.ReserveTimeSlot();
        var resultObject = GetObjectResultContent<ReservationResponse>(result);
        Assert.False(resultObject.AppointmentScheduled);
    }

    private static T GetObjectResultContent<T>(ActionResult<T> result)
    {
        return (T)((ObjectResult)result.Result).Value;
    }
}