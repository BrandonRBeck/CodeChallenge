using CodeChallenge.Models;
using CodeChallenge.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodeChallenge.Controllers;

[ApiController]
[Route("brandon-com/henry-meds/reservation")]
public class ReservationController : Controller
{
    private readonly IReservationService _reservationService;

    public ReservationController(
                IReservationService reservationService
        )
    {
        _reservationService = reservationService;
    }

    [HttpGet]
    [Route("availability")]
    public async Task<ActionResult<ReservationResponse>> GetAvailabilityForProvider()
    {
        return Ok();
    }

    [HttpPost]
    [Route("submit-availability")]
    public async Task<ActionResult<ReservationResponse>> SubmitAvailability()
    {
        return Ok();
    }

    [HttpPost]
    [Route("reserve")]
    public async Task<ActionResult<ReservationResponse>> ReserveTimeSlot()
    {
        return BadRequest();
    }

    [HttpPost]
    [Route("confirm")]
    public async Task<ActionResult<ReservationResponse>> ConfirmTimeslot()
    {
        return Ok();
    }

}