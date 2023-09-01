using CodeChallenge.RequestModel;
using CodeChallenge.ResponseModel;
using CodeChallenge.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodeChallenge.Controllers;

[ApiController]
[Route("brandon-com/henry-meds/users")]
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(
                IUserService userService
        )
    {
        _userService = userService;
    }

    //I did not have near enough time to flesh out the user controller, especially around error handling, response types...
    [HttpGet]
    public async Task<ActionResult<ReservationResponse>> GetUser([FromQuery] Guid userId)
    {
        var user = await _userService.GetUserById(userId);
        return Ok(user);
    }

    [HttpPost]
    public async Task<ActionResult<ReservationResponse>> AddUser([FromBody] UserRequestModel model)
    {
        var user = await _userService.AddUser(model);
        return Ok(user);
    }

}