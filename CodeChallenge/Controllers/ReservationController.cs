using CodeChallenge.Constants;
using CodeChallenge.RequestModel;
using CodeChallenge.ResponseModel;
using CodeChallenge.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodeChallenge.Controllers;

[ApiController]
[Route("brandon-com/henry-meds/reservation")]
public class ReservationController : Controller
{
    private readonly IReservationService _reservationService;
    private readonly IUserService _userService;

    public ReservationController(
                IReservationService reservationService,
                IUserService userService
        )
    {
        _reservationService = reservationService;
        _userService = userService;
    }

    [HttpGet]
    [Route("availability")]
    public async Task<ActionResult<ReservationResponse>> GetAvailabilityForProvider([FromQuery] Guid providerId)
    {
        var provider = await _userService.GetUserById(providerId);
        if(provider == null)
        {
            return NotFound($"No Provider Found for provider Id {providerId}");
        }
        if(provider.Type != UserConstants.UserTypeProvider)
        {
            return BadRequest($"Requested User is not of type 'Provider'");
        }

        var response = await _reservationService.GetProviderAvailabilty(provider);

        return Ok(response);
    }

    [HttpPost]
    [Route("submit-availability")]
    public async Task<ActionResult> SubmitAvailability([FromBody] ProviderAvailabilityModel availability)
    {
        var provider = await _userService.GetUserById(availability.ProviderId);
        if (provider == null)
        {
            return NotFound($"No Provider Found for provider Id {availability.ProviderId}");
        }
        if (provider.Type != UserConstants.UserTypeProvider)
        {
            return BadRequest($"Requested User is not of type 'Provider'");
        }

        await _reservationService.ScheduleProviderAvailability(provider, availability);

        return Ok();
    }

    [HttpPost]
    [Route("reserve")]
    public async Task<ActionResult<ReservationResponse>> ReserveTimeSlot([FromBody] ReservationRequestModel reservationRequest)
    {
        try
        {
            var response = await _reservationService.ReserveTimeSlot(reservationRequest);
            return Ok(response);
        }
        catch(BadHttpRequestException e)
        {
            return BadRequest(e.Message);
        }

       
    }

    [HttpPost]
    [Route("confirm")]
    public async Task<ActionResult<ConfirmationResponse>> ConfirmTimeslot([FromBody] ConfirmationRequestModel confirmRequest)
    {
        try
        {
            var response = await _reservationService.ConfirmTimeslot(confirmRequest);
            return Ok(response);
        }
        catch(BadHttpRequestException e)
        {
            return BadRequest(e.Message);
         }
    }



}