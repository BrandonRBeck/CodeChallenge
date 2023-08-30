using CodeChallenge.Models;

namespace CodeChallenge.Services;

public interface ICodeChallengeService
{
    Task<ChallengeResponseObject> ReverseString(string userInput);
}
