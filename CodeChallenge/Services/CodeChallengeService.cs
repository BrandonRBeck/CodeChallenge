using CodeChallenge.Models;

namespace CodeChallenge.Services;

public class CodeChallengeService : ICodeChallengeService
{
    public async Task<ChallengeResponseObject> ReverseString(string userInput)
    {
        await Task.CompletedTask;
        var inputAsArray = userInput.ToCharArray();
        Array.Reverse(inputAsArray);
        var result = new string(inputAsArray);
        return new ChallengeResponseObject
        {
            Response = result
        };
    }
}
