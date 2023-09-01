using CodeChallenge.Persistence.DBModel;
using CodeChallenge.RequestModel;

namespace CodeChallenge.Services;

public interface IUserService
{
    Task<User> GetUserById(Guid id);

    Task<User> AddUser(UserRequestModel model);
}
