using CodeChallenge.Models;
using CodeChallenge.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodeChallenge.Controllers;

[ApiController]
[Route("BrandonCom/HenryMeds/Users")]
public class UserController : Controller
{
    private readonly IUserService _userService;

    public UserController(
                IUserService userService
        )
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<ReservationResponse>> GetUser([FromQuery] string userInput)
    {
        return Ok();
    }

    [HttpPost]
    public async Task<ActionResult<ReservationResponse>> AddUser([FromBody] string userInput)
    {
        return Ok();
    }

}