using CodeChallenge.Models;

namespace CodeChallenge.Services;

public interface ICodeChallengeService
{
    Task<ChallengeResponseObject> GetTheResponse(string userInput);
}
