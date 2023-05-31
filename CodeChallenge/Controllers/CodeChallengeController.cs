﻿using CodeChallenge.Models;
using CodeChallenge.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodeChallenge.Controllers;

[ApiController]
[Route("BrandonCom/ApiTest")]
public class CodeChallengeController : Controller
{
    private readonly ICodeChallengeService _codeChallengeService;

    public CodeChallengeController(
                ICodeChallengeService codeChallengeService
        )
    {
        _codeChallengeService = codeChallengeService;
    }

    [HttpGet]
    [Route("ReverseString")]
    public async Task<ActionResult<ChallengeResponseObject>> InitiateChallenge([FromQuery] string userInput) 
    {
        var response = await _codeChallengeService.GetTheResponse(userInput);
        return Ok(response);
    }
}
