using CodeChallenge.Controllers;
using CodeChallenge.Models;
using CodeChallenge.Services;
using Microsoft.AspNetCore.Mvc;

namespace CodeChallengeTest;

public class CodeChallengeControllerTest
{
    [Fact]
    public async void TestControllerEndpointSuccess()
    {
        var codeChallengeService = new CodeChallengeService();
        var controller = new CodeChallengeController(codeChallengeService);
        var result = await controller.InitiateChallenge();
        var resultObject = GetObjectResultContent<ChallengeResponseObject>(result);
        Assert.NotNull(resultObject.Response);
        Assert.Equal("test string", resultObject.Response);
    }

    [Fact]
    public async void TestControllerEndpointFailure()
    {
        var codeChallengeService = new CodeChallengeService();
        var controller = new CodeChallengeController(codeChallengeService);
        var result = await controller.InitiateChallenge();
        var resultObject = GetObjectResultContent<ChallengeResponseObject>(result);
        Assert.NotNull(resultObject.Response);
        Assert.NotEqual("test stuff", resultObject.Response);
    }


    private static T GetObjectResultContent<T>(ActionResult<T> result)
    {
        return (T)((ObjectResult)result.Result).Value;
    }
}
