using CodeChallenge.Controllers;
using CodeChallenge.Models;
using CodeChallenge.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit.Abstractions;

namespace CodeChallengeTest;

public class CodeChallengeControllerTest
{
    private readonly ITestOutputHelper _output;

    public CodeChallengeControllerTest(ITestOutputHelper output)
    {
        _output = output;
    }

    [Theory]
    [InlineData("test")]
    public async void TestControllerEndpointSuccess(string input)
    {
        var codeChallengeService = new CodeChallengeService();
        var controller = new CodeChallengeController(codeChallengeService);
        var result = await controller.ReverseString(input);
        var resultObject = GetObjectResultContent<ChallengeResponseObject>(result);
        Assert.NotNull(resultObject.Response);
        Assert.Equal("tset", resultObject.Response);
    }

    [Theory]
    [InlineData("test")]
    public async void TestControllerEndpointFailure(string input)
    {
        var codeChallengeService = new CodeChallengeService();
        var controller = new CodeChallengeController(codeChallengeService);
        var result = await controller.ReverseString(input);
        var resultObject = GetObjectResultContent<ChallengeResponseObject>(result);
        Assert.NotNull(resultObject.Response);
        Assert.NotEqual("test stuff", resultObject.Response);
    }

    private static T GetObjectResultContent<T>(ActionResult<T> result)
    {
        return (T)((ObjectResult)result.Result).Value;
    }
}